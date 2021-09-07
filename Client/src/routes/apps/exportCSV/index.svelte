<script lang="ts">
  import { json2csv } from "json-2-csv";
  import { DeliveryClient, DeliveryError } from "@kentico/kontent-delivery";
  import type {
    ContentItem,
    ContentTypeSystemAttributes,
    GenericElement,
    Language,
  } from "@kentico/kontent-delivery";
  import { delay, fromPairs, replace } from "lodash";
  import { fade } from "svelte/transition";
  import jwt_decode from "jwt-decode";

  import Filter from "../../../shared/components/filter.svelte";
  import { translate } from "../../../shared/stores/translate";
  import { localStorage } from "../../../shared/stores/localStorage";
  import translations from "./_resources";

  enum LocalStorageKeys {
    SavedExport = "ExportCsv:SavedExport:",
  }

  interface ISuperType {
    system: ContentTypeSystemAttributes;
    elements: GenericElement[];
  }

  const systemElements: GenericElement[] = [
    { codename: "id", name: "Id", type: "system", options: [] },
    {
      codename: "codename",
      name: "Codename",
      type: "system",
      options: [],
    },
    {
      codename: "content_type",
      name: "Content type",
      type: "system",
      options: [],
    },
    {
      codename: "last_modified",
      name: "Last modified date",
      type: "system",
      options: [],
    },
    { codename: "url", name: "Preview URL", type: "system", options: [] },
    { codename: "collection", name: "Collection", type: "system", options: [] },
    {
      codename: "cms_link",
      name: "Link to item in Kontent",
      type: "system",
      options: [],
    },
  ];

  const deliveryClient = ({
    projectId,
    previewApiKey,
    secureApiKey,
  }: Partial<{
    projectId: string;
    previewApiKey: string;
    secureApiKey: string;
  }>) =>
    new DeliveryClient({
      projectId,
      previewApiKey,
      secureApiKey,
      globalQueryConfig: {
        usePreviewMode: previewApiKey !== undefined,
        useSecuredMode: secureApiKey !== undefined,
      },
    });

  let genericInput: string = "";
  let projectId: string = "";
  let securedDelivery: boolean = false;
  let secureApiKey: string;
  let previewDelivery: boolean = false;
  let previewApiKey: string;
  let superTypes: ISuperType[];
  let languages: Language[];

  let superTypesToExport: ISuperType[] = [];
  let exportSchema: {
    [key: string]: {
      superType: ISuperType;
      elements: GenericElement[];
      previewUrl: string;
    };
  } = {};

  $: {
    for (const superType of superTypesToExport) {
      if (!exportSchema.hasOwnProperty(superType.system.codename)) {
        exportSchema[superType.system.codename] = {
          superType,
          elements: systemElements,
          previewUrl: "",
        };
      }
    }

    for (const exportItem of Object.values(exportSchema)) {
      const { superType } = exportItem;

      if (!superTypesToExport.includes(superType)) {
        delete exportSchema[superType.system.codename];
      }
    }

    exportSchema = exportSchema;
  }

  let saving: boolean = false;

  $: savedExport =
    superTypes &&
    localStorage<{
      [key: string]: {
        elementCodenames: string[];
        previewUrl: string;
      };
    }>(LocalStorageKeys.SavedExport + projectId);

  let result: string = "";

  $: genericInput !== undefined && loadKeys();

  const loadKeys = () => {
    superTypes = undefined;
    languages = undefined;
    superTypesToExport = [];
    exportSchema = {};
    securedDelivery = false;
    secureApiKey = undefined;
    previewDelivery = false;
    previewApiKey = undefined;

    try {
      const parsedToken = jwt_decode<{ project_id: string; aud: string }>(
        genericInput
      );

      projectId = parsedToken.project_id.replace(
        /([a-f0-9]{8})([a-f0-9]{4})([a-f0-9]{4})([a-f0-9]{4})([a-f0-9]{12})/,
        "$1-$2-$3-$4-$5"
      );

      switch (parsedToken.aud) {
        case "preview.deliver.kenticocloud.com":
          previewApiKey = genericInput;
          previewDelivery = true;
          break;
        case "deliver.kenticocloud.com":
          secureApiKey = genericInput;
          securedDelivery = true;
          break;
      }
    } catch (error) {
      projectId = genericInput;
    }

    if (projectId !== "") {
      loadKontent();
    }
  };

  const loadKontent = async () => {
    try {
      await loadTypesLanguagesAndSchema();
    } catch (error) {
      if (error instanceof DeliveryError && error.errorCode === 3) {
        securedDelivery = true;
      }
    }
  };

  const loadTypesLanguagesAndSchema = async () => {
    const typesRequest = deliveryClient({
      projectId,
      secureApiKey,
      previewApiKey,
    }).types();

    superTypes = (await typesRequest.toPromise()).types.map((type) => ({
      system: type.system,
      elements: [...systemElements, ...type.elements],
    }));

    const languagesRequest = deliveryClient({
      projectId,
      secureApiKey,
      previewApiKey,
    }).languages();

    languages = (await languagesRequest.toPromise()).languages;

    if ($savedExport) {
      for (const savedItem of Object.entries($savedExport)) {
        const [id, { elementCodenames, previewUrl }] = savedItem;

        const superType = superTypes.find(
          (superType) => superType.system.id === id
        );

        if (superType) {
          superTypesToExport.push(superType);

          const elements = [];

          for (const elementCodename of elementCodenames) {
            const element = superType.elements.find(
              (element) => element.codename === elementCodename
            );

            if (element) {
              elements.push(element);
            }
          }

          exportSchema[superType.system.codename] = {
            superType,
            elements,
            previewUrl,
          };
        }
      }

      exportSchema = exportSchema;
    }
  };

  const saveData = () => {
    saving = true;

    const saveSchema = Object.values(exportSchema).map(
      ({ superType, elements, previewUrl }) => [
        superType.system.id,
        {
          elementCodenames: elements.map((element) => element.codename),
          previewUrl,
        },
      ]
    );

    savedExport.set(fromPairs(saveSchema));

    delay(() => (saving = false), 150);
  };

  const exportData = async () => {
    result = "";

    let request = deliveryClient({
      projectId,
      secureApiKey,
      previewApiKey,
    }).items();

    if (superTypesToExport.length > 0) {
      request = request.types(
        superTypesToExport.map((type) => type.system.codename)
      );
    }

    const allElements = new Set<string>();

    for (const exportItem of Object.values(exportSchema)) {
      const { superType, elements } = exportItem;

      for (const element of elements) {
        if (element.type === "system") {
          if (element.codename === systemElements[4].codename) {
            const urlSlugElement = superType.elements.find(
              (element) => element.type === "url_slug"
            );

            if (urlSlugElement) {
              allElements.add(urlSlugElement.codename);
            }
          }

          continue;
        }

        allElements.add(element.codename);
      }
    }

    if (allElements.size > 0) {
      request = request.elementsParameter([...allElements.values()]);
    }

    const reduceItem = (item: ContentItem) => {
      const reducedItem = {};

      const { elements, previewUrl } = exportSchema[item.system.type];

      for (const element of elements) {
        let value = item._raw.elements[element.codename]?.value;

        if (element.type === "system") {
          switch (element.codename) {
            case systemElements[0].codename:
              value = item.system.id;
              break;
            case systemElements[1].codename:
              value = item.system.codename;
              break;
            case systemElements[2].codename:
              value = item.system.type;
              break;
            case systemElements[3].codename:
              value = item.system.lastModified.toLocaleDateString();
              break;
            case systemElements[4].codename:
              value = previewUrl;

              value = replace(
                value,
                new RegExp("{URLslug}", "gi"),
                Object.values(item._raw.elements).find(
                  (element) => element.type === "url_slug"
                )?.value
              );
              value = replace(
                value,
                new RegExp("{Lang}", "gi"),
                item.system.language
              );
              value = replace(
                value,
                new RegExp("{Codename}", "gi"),
                item.system.codename
              );
              value = replace(
                value,
                new RegExp("{ItemId}", "gi"),
                item.system.id
              );
              break;
            case systemElements[5].codename:
              value = item.system.collection;
              break;
            case systemElements[6].codename:
              const language = languages.find(
                (language) => language.system.codename === item.system.language
              );

              value = `https://app.kontent.ai/${projectId}/content-inventory/${language.system.id}/content/${item.system.id}`;
              break;
          }
        }

        if (value !== undefined) {
          reducedItem[element.codename] = value;
        }
      }

      return reducedItem;
    };

    const data = (await request.toPromise()).items.map(reduceItem);

    json2csv(data, (error, csv) => (result = csv), {
      emptyFieldValue: "",
      excelBOM: true,
    });
  };

  const getFilename = (prefix: string, middle: string, suffix: string) => {
    const sanitize = (value: string) =>
      replace(value, /[\/, ]/g, "_").slice(0, 50);

    return `${sanitize(prefix)}_${sanitize(middle)}_${sanitize(suffix)}`;
  };

  const downloadFile = (body: string, filename: string, extension = "csv") => {
    const blob = new Blob([body]);

    const link = document.createElement("a");

    link.href = URL.createObjectURL(blob);
    link.download = `${filename}.${extension}`;
    link.style.visibility = "hidden";

    document.body.appendChild(link);

    link.click();

    document.body.removeChild(link);

    URL.revokeObjectURL(link.href);
  };

  const t = translate(translations);
</script>

<section>
  <div class="group">
    <div class="group column item">
      <h2>{$t`export`}</h2>
      <p>
        {@html $t`exportDescription1`}
      </p>
      <label class="group column">
        <b>
          {$t`projectId`}
        </b>
        <input
          type="text"
          placeholder={$t`projectIdPlaceholder`}
          bind:value={genericInput} />
      </label>
      {#if securedDelivery && !superTypes}
        <b>
          {$t`securedDeliveryDescription`}
        </b>
      {/if}
      {#if previewDelivery && !superTypes}
        <b>
          {$t`previewDeliveryDescription`}
        </b>
      {/if}
      {#key superTypes}
        {#if superTypes && languages}
          <p>
            {@html $t`exportDescription2`}
          </p>
          <p>
            {$t`exportUrlsDescription`}
            <a
              href={`https://app.kontent.ai/${projectId}/settings/preview-urls`}
              target="_blank">{$t`exportUrlsLink`}</a
            >.
          </p>
          <div class="group column">
            <label class="group column item" for="contentType">
              <b>
                {$t`exportTypes`}
              </b>
              <Filter
                bind:value={superTypesToExport}
                id="contentType"
                placeholder={$t`exportTypesPlaceholder`}
                values={superTypes}
                mapOption={(contentType) => ({
                  display: contentType.system.name,
                  key: contentType.system.id,
                  filter: contentType.system.codename.toLowerCase(),
                })}
                getValue={(rawValue) =>
                  superTypes.find(
                    (type) => type.system.codename === rawValue
                  )} />
            </label>
            {#each Object.values(exportSchema) as { superType, elements, previewUrl } (superType)}
              <div class="export" transition:fade={{ duration: 150 }}>
                <h3>{superType.system.name}</h3>
                <label class="group column item" for={superType.system.id}>
                  <b>
                    {$t`exportElements`}
                  </b>
                  <Filter
                    bind:value={elements}
                    id={superType.system.id}
                    placeholder={$t`exportElementsPlaceholder`}
                    values={superType.elements}
                    mapOption={(element) => ({
                      display: element.name,
                      key: `${element.type}${element.codename}`,
                      filter: element.name.toLowerCase(),
                    })}
                    getValue={(rawValue) =>
                      superType.elements.find(
                        (element) => element.codename === rawValue
                      )}
                    setValueOverride={(setValue, newValue, add) => {
                      setValue(newValue, add);
                      exportSchema = exportSchema;
                    }} />
                </label>
                <label class="group column">
                  <b>
                    {$t`exportUrl`}
                  </b>
                  <input
                    type="text"
                    bind:value={previewUrl}
                    placeholder={$t`exportUrlPlaceholder`} />
                </label>
              </div>
            {/each}
          </div>
          <div class="group">
            <button
              class="item"
              on:click={exportData}
              disabled={superTypesToExport.length === 0 ||
                !Object.values(exportSchema).some(
                  (exportItem) => exportItem.elements.length > 0
                )}>{$t`export`}</button>
            <button
              class="item"
              on:click={saveData}
              disabled={superTypesToExport.length === 0 ||
                saving ||
                !Object.values(exportSchema).some(
                  (exportItem) => exportItem.elements.length > 0
                )}>{$t`save`}</button>
          </div>
        {/if}
      {/key}
    </div>
    <div class="result group column">
      {#if superTypes && languages && result !== ""}
        <div class="group column item">
          <h2>{$t`result`}</h2>
          <p>
            {@html $t`exportDescription3`}
          </p>
          <textarea class="item" readonly bind:value={result} />
        </div>
        <div class="group">
          <button
            class="item"
            on:click={() =>
              downloadFile(
                result,
                getFilename(
                  $t`filename`,
                  superTypesToExport.map((type) => type.system.name).join("_"),
                  new Date().toLocaleString()
                )
              )}>{$t`download`}</button>
        </div>
      {/if}
    </div>
  </div>
</section>

<style>
  section {
    margin-top: 1em;
  }

  h2 + p {
    margin-top: 0;
  }

  .group :global(sup) {
    display: inline-block;
    border-style: solid;
    color: #4c4d52;
    border-color: #919194;
    padding: 0.1em 0.2em;
    font-size: 0.85em;
    border-width: 0.1em;
    border-radius: 0.25em;
    vertical-align: initial;
    line-height: 1.1em;
  }

  input[type="text"] {
    outline: none;
    background: none;
    margin: 0.2em 0em 0em 0em;
    color: #151515;
    font-family: inherit;
    border: 1px solid #a8a8a8;
    border-radius: calc((1vh + 1vw) * 1);
    padding: calc((1vh + 1vw) * 0.3);
  }

  input:focus {
    border-color: #141619;
  }

  ::-webkit-input-placeholder {
    font-style: italic;
    opacity: 0.5;
  }

  ::-moz-placeholder {
    font-style: italic;
    opacity: 0.5;
  }

  :-ms-input-placeholder {
    font-style: italic;
    opacity: 0.5;
  }

  :-moz-placeholder {
    font-style: italic;
    opacity: 0.5;
  }

  .export {
    margin: calc((1vh + 1vw) * 0.5) 0 0 calc((1vh + 1vw) * 0.5);
    border-radius: calc((1vh + 1vw) * 0.5);
    padding: calc((1vh + 1vw) * 0.5);
    background-color: rgba(175, 197, 233, 0.2);
  }

  button {
    border: none;
    outline: none;
    cursor: pointer;
    height: 2.5em;
    padding: 0 0.5em;
    font-weight: 700;
    border-radius: 100em;
    color: rgb(255, 255, 255);
    fill: rgb(255, 255, 255);
    margin: 0 0 0 0.5em;
    font-family: inherit;
    text-transform: uppercase;
    background: rgb(219, 60, 0);
    box-shadow: rgb(244 92 35 / 14%) 0px 8px 14px 2px,
      rgb(244 92 35 / 12%) 0px 6px 20px 5px,
      rgb(244 92 35 / 20%) 0px 8px 10px -5px;
  }

  button + button {
    margin-left: 1em;
  }

  button:hover {
    background-color: #a82e00;
  }

  button:disabled {
    color: #ffffff;
    background-color: #adadad;
  }

  button:hover:disabled {
    cursor: not-allowed;
  }

  textarea {
    resize: none;
    font-size: 1em;
    font-family: inherit;
    border: none;
    border-radius: 0.5em;
    padding: 0.5em;
    background: rgba(175, 197, 233, 0.2);
  }

  textarea:focus {
    outline: none;
  }

  .result {
    padding: 0 0 0 1em;
    position: sticky;
    top: 1em;
    height: calc(100vh - 2em);
    width: 40em;
  }
</style>
