<script context="module" lang="ts">
  declare const CustomElement: ICustomElement;
</script>

<script lang="ts">
  import { createEventDispatcher, onMount } from "svelte";
  import type { IContext, ICustomElement } from "./customElement";
  import { round } from "lodash";

  export let value: any;
  export let config: {};
  export let context: IContext;
  export let height: number;
  export let disabled: boolean = true;

  let root: HTMLDivElement;
  let resizeObserver: ResizeObserver;
  let invalid: boolean;
  let customElement: ICustomElement;

  const dispatch = createEventDispatcher();

  onMount(() => {
    invalid = window.self === window.top;

    if (invalid) {
      return;
    }

    resizeObserver = new ResizeObserver((entries) => {
      if (height !== undefined) {
        return;
      }

      for (let entry of entries) {
        if (entry.contentBoxSize && entry.contentBoxSize[0]) {
          customElement?.setHeight(round(entry.contentBoxSize[0].blockSize));
        } else {
          customElement?.setHeight(round(entry.contentRect.height));
        }
      }
    });
    resizeObserver.observe(root);

    customElement = CustomElement;

    customElement.init((element, elementContext) => {
      element.value && (value = JSON.parse(element.value));
      config = element.config ?? {};
      context = elementContext;
      disabled = element.disabled;

      dispatch("ready");
    });

    customElement.onDisabledChanged(
      (elementDisabled) => (disabled = elementDisabled)
    );

    return () => resizeObserver.disconnect();
  });

  $: {
    value !== undefined &&
      !disabled &&
      customElement?.setValue(JSON.stringify(value));
  }

  $: {
    height !== undefined && customElement?.setHeight(round(height));
  }
</script>

<svelte:head
  ><script src="https://app.kontent.ai/js-api/custom-element.js">
  </script></svelte:head>

<div bind:this={root}>
  {#if !invalid}
    {#if config}
      <slot />
    {:else}
      <slot name="loading" />
    {/if}
  {:else}
    <slot name="invalid" />
  {/if}
</div>

<!-- Workaround for :global() styles not being removed: https://github.com/sveltejs/svelte/issues/5530 -->
<div>
  <body />
</div>

<!-- Workaround for :global() styles not being removed: https://github.com/sveltejs/svelte/issues/5530 -->
<style>
  @import url("https://fonts.googleapis.com/css?family=Source+Sans+Pro:400,700,400italic,700italic");

  body {
    font-family: Source Sans Pro, sans-serif;
    overflow: hidden;
  }

  div body {
    display: none;
  }
</style>
