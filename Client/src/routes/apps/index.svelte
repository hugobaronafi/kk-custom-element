<script context="module" lang="ts">
  import type { Preload } from "@sapper/common";

  import type { SvelteConstructor } from "../../shared/kontent";
  import { replaceComponents } from "../../shared/kontent";
  import type { ICode } from "../../shared/models/Code";
  import { Code as CodeModel } from "../../shared/models/Code";
  import type { IApp } from "../../shared/models/App";
  import type { IIcon } from "../../shared/models/Icon";

  export const preload: Preload = async function (this, page, session) {
    const apps = await (await this.fetch("kontent/apps")).json();
    const icons = await (await this.fetch("kontent/icons")).json();

    return {
      apps: apps.apps,
      components: apps.components,
      icons,
    };
  };
</script>

<script lang="ts">
  import sortArray from "sort-array";
  import { onMount, tick } from "svelte";
  import { stores } from "@sapper/app";

  import Code from "../../shared/components/code.svelte";
  import { translate } from "../../shared/stores/translate";

  export let apps: IApp[];
  export let components: Map<string, ICode>;
  export let icons: IIcon[];

  let selectedApp: IApp;

  const { session } = stores();

  const replaceMap = new Map<string, SvelteConstructor>([
    [CodeModel.codename, (args) => new Code(args)],
  ]);

  onMount(() => {
    if (window.location.hash) {
      selectedApp = apps.find(
        (app) => app.codename == window.location.hash.slice(1)
      );
    }
  });

  $: selectedApp && scrollTo();

  const scrollTo = async () => {
    if (selectedApp) {
      const listItem = document.getElementById(selectedApp.codename);

      if (listItem) {
        await tick();

        window.scrollTo({
          top: listItem.offsetTop,
          behavior: "smooth",
        });
      }
    }
  };

  let filter: string = "";

  $: if (filter) {
    selectedApp = undefined;
    history.replaceState(
      undefined,
      undefined,
      `${window.location.origin}${window.location.pathname}`
    );
  }

  $: sortedApps = sortArray(
    apps.filter((app) => {
      if (filter === "") {
        return true;
      }

      const matches = (value: string) => value.match(new RegExp(filter, "gi"));

      if (matches(app.name)) {
        return true;
      }

      if (app.tags.some((tag) => matches(tag.name) || matches(tag.synonyms))) {
        return true;
      }

      if (matches(app.description)) {
        return true;
      }

      return false;
    }),
    {
      by: ["name"],
    }
  );

  const gitHubIcon = icons.find((icon) => icon.codename == "github_icon");

  const t = translate([$session.translations]);
</script>

<section>
  <div class="list">
    <div class="filter">
      <input type="text" placeholder={$t("filter_apps")} bind:value={filter} />
    </div>
    {#each sortedApps as app (app.name)}
      <div class="group" id={app.codename} class:selected={selectedApp == app}>
        <div
          class="content"
          on:click={() => {
            if (selectedApp !== app) {
              selectedApp = app;
              history.replaceState(
                undefined,
                undefined,
                `${window.location.origin}${window.location.pathname}#${app.codename}`
              );
            }
          }}>
          <h2 class="name">{app.name}</h2>
          {#if selectedApp == app}
            {#each sortArray(app.tags, { by: ["name"] }) as tag (tag.codename)}
              <span class="tag">{tag.name}</span>
            {/each}
            <div
              class="description"
              use:replaceComponents={{ components, replaceMap }}>
              {@html app.description}
            </div>
            <a class="badge" href={app.github}
              >{@html gitHubIcon.svg}{$t("github")}</a>
          {/if}
        </div>
        <div class="image" />
      </div>
    {/each}
  </div>
</section>

<style>
  .filter {
    display: flex;
  }

  .filter input[type="text"] {
    outline: none;
    background: none;
    margin: 0.2em 0em 1em 0em;
    color: #151515;
    font-family: inherit;
    border: 1px solid #a8a8a8;
    border-radius: calc((1vh + 1vw) * 1);
    padding: calc((1vh + 1vw) * 0.3);
    flex: 1;
  }

  input:focus {
    border-color: #141619;
  }

  .list {
    flex: 1;
    z-index: 1;
  }

  .group .content {
    flex: 2;
    padding: 1em;
    position: relative;
    overflow: hidden;
    border-radius: 1em;
  }

  .group:not(.selected) .content:before {
    width: 300%;
    height: 300%;
    content: "";
    left: -1em;
    top: -1em;
    position: absolute;
    background: linear-gradient(
      160deg,
      rgba(175, 197, 233, 0.2),
      rgba(175, 197, 233, 0.2) 35.9%,
      #db3c00 36%,
      #db3c00
    );
    transition: all 0.5s;
    transform: translate(0%, 0%);
    z-index: -1;
  }

  .group:not(.selected) .content .description {
    display: none;
  }

  .group:not(.selected) .content:hover {
    cursor: pointer;
  }

  .group:not(.selected) .content:hover:before {
    transform: translate(-45%, -45%);
  }

  .group.selected .content {
    background: rgba(175, 197, 233, 0.2);
  }

  .group.selected .content .description {
    display: block;
  }

  .name {
    margin: 0;
  }

  .group.selected .name {
    margin-bottom: 0.5em;
  }

  .tag {
    font-size: 0.8em;
    background: #d0d0d0;
    margin-right: 0.2em;
    text-transform: uppercase;
    border-radius: 100em;
    padding: 0.2em 0.6em;
    color: white;
    font-weight: 500;
    background: #db3c00;
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

  .badge {
    background: #db3c00;
    margin-right: 0.2em;
    border-radius: 100em;
    padding: 0.2em 0.6em;
    color: #ffffff;
    fill: #ffffff;
    font-weight: 500;
    text-decoration: none;
  }

  .badge:hover {
    background: #a82e00;
  }

  .group :global(.badge svg) {
    height: 0.9em;
    padding: 0 0.4em 0 0;
  }

  .image {
    padding-left: 1em;
    flex: 3;
  }

  @media (max-width: 800px) {
    .group {
      flex-direction: column;
    }

    .image {
      padding: 1em 0 0;
    }

    .image {
      position: relative;
      left: initial;
      max-width: 100%;
    }
  }
</style>
