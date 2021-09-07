<script context="module" lang="ts">
  export interface IListItem {
    value: string;
  }
</script>

<script lang="ts">
  import { fade } from "svelte/transition";

  import Invalid from "../../../shared/components/customElement/invalid.svelte";
  import CustomElement from "../../../shared/components/customElement/customElement.svelte";
  import { translate } from "../../../shared/stores/translate";
  import Loading from "../../../shared/components/loading.svelte";
  import sharedTranslations from "../../../shared/components/customElement/resources";
  import type { IContext } from "../../../shared/components/customElement/customElement";
  import translations from "./_resources";
  import ListItem from "./listItem.svelte";

  interface IListConfig {
    minimum: number;
    maximum: number;
    validator: string;
  }

  interface IListValue {
    items: IListItem[];
    valid: boolean;
  }

  let value: IListValue = { items: [], valid: false };
  let config: IListConfig;
  let context: IContext;
  let disabled: boolean;

  let invalid: boolean = false;
  let invalidCount: boolean = false;

  $: atLimit =
    config?.maximum !== undefined && value.items.length === config.maximum;

  const addItem = () => {
    value.items = [...value.items, { value: "" }];
  };

  const deleteItem = (event: CustomEvent<IListItem>) => {
    value.items = value.items.filter((oldItem) => oldItem !== event.detail);
  };

  $: {
    invalid = false;
    invalidCount = false;

    if (config?.minimum !== undefined && value.items.length < config.minimum) {
      invalid = true;
      invalidCount = true;
    }

    if (config?.maximum !== undefined && value.items.length > config.maximum) {
      invalid = true;
      invalidCount = true;
    }

    if (config?.validator !== undefined) {
      const validator = new RegExp(config.validator);

      for (const item of value.items) {
        if (!validator.test(item.value)) {
          invalid = true;
          break;
        }
      }
    }

    setInvalid(invalid);
  }

  const setInvalid = (invalid: boolean) => (value.valid = !invalid);

  $: validationTotal = config && getValidationTotal();

  const getValidationTotal = () => {
    if (config.maximum !== undefined) {
      return config.maximum;
    }

    if (config.minimum !== undefined) {
      return config.minimum;
    }
  };

  const t = translate([translations, sharedTranslations]);
</script>

<CustomElement bind:value bind:config bind:context bind:disabled>
  <div transition:fade>
    <div class="group column" transition:fade>
      {#each value.items as item (item)}
        <ListItem on:delete={deleteItem} bind:item />
      {/each}
    </div>
    <div class="group">
      {#if !atLimit}
        <button class="button" on:click={addItem}>{$t("addItem")}</button>
      {/if}
      <div class="item group column validation">
        <div class="item" />
        <div class="group">
          <div class="item">
            {#if invalid}
              <span class="dot" title={$t("notFilledInCorrectlyTitle")}>
                {$t("notFilledInCorrectly")}
              </span>
            {/if}
          </div>
          {#if validationTotal}
            <span class="item number">
              <span class:invalidCount>
                {value.items.length}</span
              >{`${"/"}${validationTotal} ${$t("items")}`}</span>
          {/if}
        </div>
      </div>
    </div>
  </div>
  <div slot="loading">
    <Loading />
  </div>
  <div slot="invalid">
    <Invalid />
  </div>
</CustomElement>

<style>
  @font-face {
    font-family: kentico-icons;
    font-style: normal;
    font-weight: 400;
    src: url("/kentico-icons-v1.6.0.woff") format("woff");
  }

  .group {
    margin: 0 !important;
  }

  .validation {
    color: #919194;
    margin: 0.6em 0 !important;
    font-size: 0.85em;
    border: 0.1em solid transparent;
  }

  .number {
    text-align: right;
  }

  .dot:before {
    padding-right: 0.25em;
    font-family: kentico-icons;
    font-size: 0.75em;
    color: #f05a22;
    content: "\e626";
  }

  .invalidCount {
    color: #f02222;
  }
</style>
