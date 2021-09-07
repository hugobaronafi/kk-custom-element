<script lang="ts">
  import { property } from "../actions/property";

  export let showActions: boolean;
  export let imageUrl: string;
  export let name: string;
  export let title: string;
  export let selected: boolean;
  export let thumbnailUrl: string;
  export let detail: string;
  export let size: number = 3;
  export let onRemove: () => void;
  export let onClick: () => void;
</script>

<div class="wrapper" use:property={["size", size]}>
  <div class="content" class:selected on:click={onClick}>
    {#if showActions}
      <div class="actions">
        <a target="_blank" rel="noopener noreferrer" href={imageUrl}>
          <i aria-hidden="true" class="icon download" />
        </a>
        {#if onRemove}
          <div on:click={onRemove}>
            <i aria-hidden="true" class="icon remove" />
          </div>
        {/if}
      </div>
    {/if}
    {#if onClick}
      <div class="preview" {title}>
        <slot name="image">
          <img class="image" alt={name} src={thumbnailUrl} />
        </slot>
      </div>
    {:else}
      <a target="_blank" rel="noopener noreferrer" href={imageUrl}>
        <div class="preview" {title}>
          <slot name="image">
            <img class="image" alt={name} src={thumbnailUrl} />
          </slot>
        </div>
      </a>
    {/if}
    {#if name || detail}
      <div class="bottom">
        {#if name}<span class="name">{name}</span>{/if}
        {#if detail}
          <div class="details"><span class="detail">{detail}</span></div>
        {/if}
      </div>
    {/if}
  </div>
</div>

<style>
  .wrapper {
    width: calc(100% / var(--size) - 1em);
    margin: 0.5em;
    display: flex;
    height: calc(40em / var(--size));
  }

  .content {
    position: relative;
    flex: 1;
    cursor: pointer;
    background-color: #f5f5f5;
    border: 0.15em solid #d0cfce;
    border-radius: 0.1em;
    transition: all 0.15s cubic-bezier(0.23, 1, 0.32, 1) 0ms;
  }

  .selected {
    outline: 0.2em solid #0a68f5;
    box-shadow: 0 0.1em 0.5em 0.15em rgba(0, 0, 0, 0.2);
  }

  .actions {
    position: absolute;
    top: 0;
    z-index: 100;
    display: flex;
    justify-content: flex-end;
    width: 100%;
    cursor: default;
    background-color: hsla(0, 0%, 96.1%, 0.85);
  }

  .preview {
    display: flex;
    align-items: center;
    justify-content: space-around;
    height: 100%;
  }

  .image {
    max-width: 100%;
    max-height: 100%;
  }

  .bottom {
    position: absolute;
    bottom: 0;
    left: 0;
    display: flex;
    width: 100%;
    background-color: hsla(0, 0%, 96.1%, 0.8);
    border-bottom-right-radius: 0.1em;
    border-bottom-left-radius: 0.1em;
    transition: all 0.15s cubic-bezier(0.23, 1, 0.32, 1) 0ms;
    padding: 0.5em;
    flex-direction: column;
  }

  .name {
    width: 100%;
    color: #424242;
    margin-bottom: 0.25em;
    font-size: 0.9em;
    line-height: 1.25;
    word-wrap: break-word;
    white-space: nowrap;
    overflow: hidden;
    text-overflow: ellipsis;
  }

  .name:hover {
    overflow: visible;
    white-space: normal;
  }

  .details {
    font-size: 0.8em;
    line-height: 1.1em;
    opacity: 1;
    text-overflow: ellipsis;
    max-height: 1em;
    white-space: nowrap;
    overflow: hidden;
  }

  .detail:after {
    color: #424242;
    content: "A0B7A0";
  }

  .detail:last-of-type:after {
    content: "";
  }
</style>
