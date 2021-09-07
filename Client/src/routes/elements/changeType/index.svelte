<script lang="ts">
  import { fade } from "svelte/transition";
  import wretch from "wretch";
  import moment from "moment";
  import { round } from "lodash";

  import type {
    ElementType,
    IContentItem,
    IContentType,
  } from "../../../shared/management";
  import Invalid from "../../../shared/components/customElement/invalid.svelte";
  import CustomElement from "../../../shared/components/customElement/customElement.svelte";
  import { translate } from "../../../shared/stores/translate";
  import Loading from "../../../shared/components/loading.svelte";
  import sharedTranslations from "../../../shared/components/customElement/resources";
  import type { IContext } from "../../../shared/components/customElement/customElement";
  import translations from "./_resources";

  interface IGetTypesResponse {
    currentType: IContentType;
    otherTypes: IContentType[];
  }

  interface IChangeTypeResponse {
    totalApiCalls: number;
    totalMilliseconds: number;
    newItem: IContentItem;
    updatedItems: IContentItem[];
  }

  interface IChangeTypeConfig {
    changeTypeEndpoint: string;
    getTypesEndpoint: string;
  }

  const typeMap: { [key in ElementType]: ElementType[] } = {
    asset: ["asset"],
    snippet: ["snippet"],
    custom: ["custom"],
    date_time: ["date_time"],
    modular_content: ["modular_content"],
    number: ["number"],
    multiple_choice: ["multiple_choice"],
    rich_text: ["rich_text"],
    taxonomy: ["taxonomy"],
    url_slug: ["text", "url_slug", "custom"],
    guidelines: ["guidelines", "text"],
    text: ["text", "date_time", "custom", "number", "rich_text"],
  };

  let value = null;
  let config: IChangeTypeConfig;
  let context: IContext;
  let disabled: boolean;

  let loading: boolean = false;
  let responseError: any;
  let getTypesResponse: IGetTypesResponse;
  let typeId: string = "";
  let elementMappings: Map<string, string> = new Map<string, string>();
  let changeTypeResponse: IChangeTypeResponse;

  $: config && context && loadTypes();

  const loadTypes = async () => {
    loading = true;

    const getTypesEndpoint = new URL(config.getTypesEndpoint);
    getTypesEndpoint.pathname += `/${context.item.codename}`;

    const request = wretch(getTypesEndpoint.toString())
      .get()
      .json<IGetTypesResponse>();

    try {
      responseError = undefined;
      getTypesResponse = await request;
    } catch (error) {
      responseError = error;
    }

    loading = false;
  };

  $: selectedType = getTypesResponse?.otherTypes.find(
    (otherType) => otherType.id === typeId
  );

  const changeType = async () => {
    loading = true;

    const changeTypeEndpoint = new URL(config.changeTypeEndpoint);
    changeTypeEndpoint.pathname += `/${context.item.codename}/${context.variant.codename}`;

    const request = wretch(changeTypeEndpoint.toString())
      .post({ elementMappings, selectedType })
      .json<IChangeTypeResponse>();

    try {
      responseError = undefined;
      changeTypeResponse = await request;
    } catch (error) {
      responseError = error;
    }

    loading = false;
  };

  $: totalTime = changeTypeResponse && getTotalTime();

  const getTotalTime = () => {
    const totalTime = moment.duration(changeTypeResponse.totalMilliseconds);
    const result = [];

    if (totalTime.hours() > 0) {
      result.push(`${totalTime.hours()} ${$t("hours")}`);
    }

    if (totalTime.minutes() > 0) {
      result.push(`${totalTime.minutes()} ${$t("minutes")}`);
    }

    if (totalTime.seconds() > 0) {
      result.push(
        `${
          totalTime.seconds() + round(totalTime.milliseconds() / 1000, 3)
        } ${$t("seconds")}`
      );
    }

    return result.join(", ");
  };

  const t = translate([translations, sharedTranslations]);
</script>

<CustomElement bind:value bind:config bind:context bind:disabled>
  {#if loading}
    <Loading />
  {:else}
    <div transition:fade>
      {#if disabled}
        <div class="group">{$t("noFunctionality")}</div>
      {/if}
      {#if responseError}
        <div class="group">{responseError}</div>
      {/if}
      {#if !disabled && getTypesResponse}
        <div class="group">
          <select class="select" bind:value={typeId}>
            <option value="">{$t("chooseType")}</option>
            {#each getTypesResponse.otherTypes as otherType (otherType.id)}
              <option value={otherType.id}>
                {otherType.name || otherType.codename}
              </option>
            {/each}
          </select>
          {#if typeId !== ""}
            <button class="button" on:click={changeType}
              >{$t("changeType")}</button>
          {/if}
        </div>
      {/if}
      {#if typeId !== ""}
        <div class="group" transition:fade>
          <div class="item"><b>{$t("elementName")}</b></div>
          <div class="item"><b>{$t("elementType")}</b></div>
          <div class="item"><b>{$t("elementSource")}</b></div>
        </div>
        <div class="group column" transition:fade>
          {#each selectedType.elements as element (element.id)}
            <div class="group">
              <div class="item">{element.name || element.codename}</div>
              <div class="item">{element.type}</div>
              <div class="item">
                <!-- svelte-ignore a11y-no-onchange -->
                <select
                  class="select"
                  value={elementMappings[element.id]}
                  on:change={(event) => {
                    const value = event.currentTarget.value;

                    if (value !== "") {
                      elementMappings[element.id] = value;
                    } else {
                      delete elementMappings[element.id];
                    }
                  }}>
                  <option value="">{$t("chooseElement")}</option>
                  {#each getTypesResponse.currentType.elements.filter(
                    (currentElement) =>
                      typeMap[element.type].some(
                        (allowedElementType) =>
                          allowedElementType === currentElement.type
                      )
                  ) as currentElement (currentElement.id)}
                    <option value={currentElement.id}>
                      {`${currentElement.name ?? currentElement.codename} (${
                        currentElement.type
                      })`}
                    </option>
                  {/each}
                </select>
              </div>
            </div>
          {/each}
        </div>
      {/if}
      {#if changeTypeResponse && !responseError}
        <div class="group" transition:fade>
          <div class="group column item">
            <div class="label">{$t("totalTime")}</div>
            <span>{totalTime}</span>
          </div>
          <div class="group column item">
            <div class="label">{$t("totalApiCalls")}</div>
            <span>{changeTypeResponse.totalApiCalls}</span>
          </div>
        </div>
        <div class="group">
          <div class="group column">
            <div class="label">{$t("newItem")}</div>
            <span>
              <a
                href={`https://app.kontent.ai/${context.projectId}/content-inventory/${context.variant.id}/content/${changeTypeResponse.newItem.id}`}
                target="_blank"
                rel="noopener noreferrer">
                {changeTypeResponse.newItem.name}
              </a>
            </span>
          </div>
        </div>
        {#if changeTypeResponse.updatedItems.length > 0}
          <div class="group">
            <div class="group column item">
              <div class="label">{$t("updatedItems")}</div>
              {#each changeTypeResponse.updatedItems as updatedItem (updatedItem.id)}
                <span>
                  <a
                    href={`https://app.kontent.ai/${context.projectId}/content-inventory/${context.variant.id}/content/${updatedItem.id}`}
                    target="_blank"
                    rel="noopener noreferrer">
                    {updatedItem.name}
                  </a>
                </span>
              {/each}
            </div>
          </div>
        {/if}
      {/if}
    </div>
  {/if}
  <div slot="loading">
    <Loading />
  </div>
  <div slot="invalid">
    <Invalid />
  </div>
</CustomElement>
