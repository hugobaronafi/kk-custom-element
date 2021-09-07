<script lang="ts">
  import { fade } from "svelte/transition";
  import wretch from "wretch";
  import { debounce, flatMap } from "lodash";

  import type { IFontAwesomeIcons } from "./_fontAwesomeIcons";
  import Invalid from "../../../shared/components/customElement/invalid.svelte";
  import CustomElement from "../../../shared/components/customElement/customElement.svelte";
  import { translate } from "../../../shared/stores/translate";
  import Loading from "../../../shared/components/loading.svelte";
  import sharedTranslations from "../../../shared/components/customElement/resources";
  import ObjectTile from "../../../shared/components/objectTile.svelte";
  import translations from "./_resources";

  interface ISearchIcon {
    name: string;
    label: string;
    style: string;
    search: string;
    unicode: string;
    svg: string;
  }

  interface IIcon {
    name: string;
    label: string;
    style: string;
    unicode: string;
    svg: string;
    cssClass: string;
  }

  interface IFontAwesomeIconConfig {
    iconsMetadataEndpoint: string;
  }

  interface IFontAwesomeIconValue {
    icon?: IIcon;
  }

  let value: IFontAwesomeIconValue = {};
  let config: IFontAwesomeIconConfig;
  let disabled: boolean;

  let listOpen: boolean = false;
  let filter: string;
  let rawFilter: string = "";
  const debounceFilter = debounce((rawFilter) => (filter = rawFilter), 1000);

  $: rawFilter !== "" && debounceFilter(rawFilter);

  let data: ISearchIcon[];

  $: config && loadData();

  const loadData = async () => {
    const rawData = await wretch(config.iconsMetadataEndpoint)
      .get()
      .json<IFontAwesomeIcons>();

    data = flatMap(Object.entries(rawData), (entry) => {
      const [name, icon] = entry;

      return icon.styles.map((style) => ({
        name,
        label: icon.label,
        style,
        search: `${name} ${
          icon.search.terms.length > 0
            ? `(${icon.search.terms.join(", ")})`
            : ""
        }`,
        unicode: icon.unicode,
        svg: icon.svg[style].raw,
      }));
    });
  };

  const closeList = () => {
    listOpen = false;
    filter = undefined;
    rawFilter = "";
  };

  $: filteredData = data && filterData(filter);

  const filterData = (filter: string) => {
    const results: ISearchIcon[] = [];

    if (filter === "") {
      return data.slice(0, 100);
    }

    const matches = (value: string) => value.match(new RegExp(filter, "gi"));

    for (const icon of data) {
      if (results.length === 100) {
        break;
      }

      if (matches(icon.search)) {
        results.push(icon);
        continue;
      }
    }

    return results;
  };

  const t = translate([translations, sharedTranslations]);
</script>

<CustomElement bind:value bind:config bind:disabled>
  <div transition:fade>
    {#if !disabled}
      <div class="group">
        {#if !listOpen}
          <button class="button" on:click={() => (listOpen = true)}>
            {$t("open")}
          </button>
        {:else}
          <button class="button" on:click={closeList}> {$t("close")} </button>
        {/if}
        {#if value.icon}
          <button
            class="button destructive"
            in:fade|local
            on:click={() => {
              value.icon = undefined;
            }}>
            {$t("clear")}
          </button>
        {/if}
      </div>
      <div class="group column">
        {#if listOpen}
          <div class="group" transition:fade>
            <label class="group column filter"
              ><div class="label">
                {$t("search")}
              </div>
              <input
                class="input"
                type="text"
                placeholder={$t("placeholder")}
                bind:value={rawFilter} />
            </label>
          </div>
          <div class="group wrap">
            {#each filteredData as icon (icon.name + icon.style)}
              <ObjectTile
                title={icon.search}
                size={6}
                onClick={() => {
                  value.icon = {
                    name: icon.name,
                    label: icon.label,
                    unicode: icon.unicode,
                    style: icon.style,
                    svg: icon.svg,
                    cssClass: `fa${icon.style[0]} fa-${icon.name}`,
                  };
                  closeList();
                }}>
                <div slot="image" class="tile">
                  {@html icon.svg}
                </div>
              </ObjectTile>
            {/each}
          </div>
        {/if}
      </div>
    {/if}
    {#if value.icon}
      <div class="group column" transition:fade>
        <div>{$t("previewDescription")}</div>
        <div class="preview">
          <div class="description">
            <h2>{value.icon.label}</h2>
          </div>
          <div class="image">
            {@html value.icon.svg}
          </div>
        </div>
      </div>
    {/if}
  </div>
  <div slot="loading">
    <Loading />
  </div>
  <div slot="invalid">
    <Invalid />
  </div>
</CustomElement>

<style>
  .filter {
    flex: 1;
  }

  .preview {
    text-align: center;
  }

  .image {
    max-width: 10em;
    margin: 0 auto;
  }

  .tile {
    width: 100%;
    height: 100%;
    display: flex;
    flex-direction: column;
  }

  .tile > :global(svg) {
    margin: 0.5em;
  }
</style>
