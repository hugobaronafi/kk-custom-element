<script lang="ts">
  import { createEventDispatcher } from "svelte";

  import type { IListItem } from "./index.svelte";
  import { translate } from "../../../shared/stores/translate";
  import sharedTranslations from "../../../shared/components/customElement/resources";
  import translations from "./_resources";

  export let item: IListItem;

  const dispatch = createEventDispatcher();

  const deleteItem = () => {
    dispatch("delete", item);
  };

  const t = translate([translations, sharedTranslations]);
</script>

<div class="group">
  <div class="item">
    <input
      type="text"
      class="input"
      placeholder={$t("itemIsEmpty")}
      bind:value={item.value} />
    <div on:click={deleteItem}>
      <i aria-hidden="true" class="icon delete" />
    </div>
  </div>
</div>

<style>
  .group {
    background-color: #fff;
    border-radius: 2px;
    box-shadow: 0 1px 3px 1px rgba(0, 0, 0, 0.2);
    margin: 0.1em;
  }

  .item {
    display: flex;
  }

  .input {
    min-width: 0;
    padding: 0.25em 0.5em;
    color: #4c4d52;
    border: 0.1em solid #d0cfce !important;
    transition: border 0.25s cubic-bezier(0.23, 1, 0.32, 1) 50ms;
    resize: vertical;
    flex: 1;
    margin: 0.2em 0.2em 0.2em 0;
  }

  @font-face {
    font-family: kentico-icons;
    font-style: normal;
    font-weight: 400;
    src: url("/kentico-icons-v1.6.0.woff") format("woff");
  }

  .icon {
    display: inline-block;
    font-family: kentico-icons;
    -webkit-font-smoothing: antialiased;
    -moz-osx-font-smoothing: grayscale;
    font-style: normal;
    font-weight: 400;
    font-variant: normal;
    text-transform: none;
    padding: 0.6em 0.8em;
    font-size: 1em;
    color: #424242;
    cursor: pointer;
  }

  .icon.delete:before {
    content: "\e6d0";
  }

  .delete:hover {
    color: #fff;
    background-color: #f02222;
  }
</style>
