<script lang="ts">
  import wretch from "wretch";
  import moment from "moment";
  import { round } from "lodash";
  import { fade } from "svelte/transition";

  import type { IContentItem } from "../../../shared/management";
  import Invalid from "../../../shared/components/customElement/invalid.svelte";
  import CustomElement from "../../../shared/components/customElement/customElement.svelte";
  import { translate } from "../../../shared/stores/translate";
  import Loading from "../../../shared/components/loading.svelte";
  import sharedTranslations from "../../../shared/components/customElement/resources";
  import type { IContext } from "../../../shared/components/customElement/customElement";
  import translations from "./_resources";

  interface IDeepCloneResponse {
    totalApiCalls: number;
    totalMilliseconds: number;
    newItems: IContentItem[];
  }

  interface IDeepCloneConfig {
    deepCloneEndpoint: string;
  }

  let value = null;
  let config: IDeepCloneConfig;
  let context: IContext;
  let disabled: boolean;

  let loading: boolean = false;
  let responseError: any;
  let response: IDeepCloneResponse;

  const clone = async () => {
    loading = true;

    const deepCloneEndpoint = new URL(config.deepCloneEndpoint);
    deepCloneEndpoint.pathname += `/${context.item.codename}/${context.variant.codename}`;

    const request = wretch(deepCloneEndpoint.toString())
      .post()
      .json<IDeepCloneResponse>();

    try {
      responseError = undefined;
      response = await request;
    } catch (error) {
      responseError = error;
    }

    loading = false;
  };

  $: totalTime = response && getTotalTime();

  const getTotalTime = () => {
    const totalTime = moment.duration(response.totalMilliseconds);
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
      {#if !disabled}
        <div class="group">
          <button class="button" on:click={clone}>{$t("clone")}</button>
        </div>
      {:else}
        <div class="group">{$t("noFunctionality")}</div>
      {/if}
      {#if responseError}
        <div class="group">{responseError}</div>
      {/if}
      {#if response && !responseError}
        <div class="group" transition:fade>
          <div class="group column item">
            <div class="label">{$t("totalTime")}</div>
            <span>{totalTime}</span>
          </div>
          <div class="group column item">
            <div class="label">{$t("totalApiCalls")}</div>
            <span>{response.totalApiCalls}</span>
          </div>
        </div>
        <div class="group">
          <div class="group column item">
            <div class="label">{$t("newItems")}</div>
            {#each response.newItems as newItem (newItem.id)}
              <span>
                <a
                  href={`https://app.kontent.ai/${context.projectId}/content-inventory/${context.variant.id}/content/${newItem.id}`}
                  target="_blank"
                  rel="noopener noreferrer">
                  {newItem.name}
                </a>
              </span>
            {/each}
          </div>
        </div>
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
