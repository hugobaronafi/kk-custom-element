<script lang="ts">
  import { fade } from "svelte/transition";
  import { onMount } from "svelte";
  import wretch from "wretch";

  import type { IHubSpotForm } from "./_hubSpot";
  import Invalid from "../../../shared/components/customElement/invalid.svelte";
  import CustomElement from "../../../shared/components/customElement/customElement.svelte";
  import { localStorage } from "../../../shared/stores/localStorage";
  import { translate } from "../../../shared/stores/translate";
  import Loading from "../../../shared/components/loading.svelte";
  import sharedTranslations from "../../../shared/components/customElement/resources";
  import translations from "./_resources";

  enum LocalStorageKeys {
    Code = "HubSpotForms:code",
    RefreshToken = "HubSpotForms:refreshToken",
  }

  interface IHubSpotFormResponse {
    forms: IHubSpotForm[];
    refreshToken: string;
  }

  interface IHubSpotFormConfig {
    formsEndpoint: string;
    clientId: string;
  }

  interface IHubSpotFormValue {
    form?: IHubSpotForm;
  }

  let value: IHubSpotFormValue = {};
  let config: IHubSpotFormConfig;
  let disabled: boolean;

  let loading: boolean = true;
  let redirectUri: string;
  let responseError: any;
  let response: IHubSpotFormResponse;
  let showForm: boolean = true;

  const code = localStorage<string>(LocalStorageKeys.Code);
  const refreshToken = localStorage<string>(LocalStorageKeys.RefreshToken);

  onMount(() => {
    redirectUri = window.location.href;

    const codeParameter = new URLSearchParams(window.location.search).get(
      "code"
    );

    if (codeParameter) {
      code.set(codeParameter);
      window.close();
    }
  });

  $: if (!$code && config) {
    window.open(
      `https://app.hubspot.com/oauth/authorize?scope=contacts%20forms&client_id=${config.clientId}&redirect_uri=${redirectUri}`,
      undefined,
      "width=800,height=600"
    );
  }

  $: $code && config && loadForms();

  const loadForms = async () => {
    let continueRequest = true;

    const request = wretch(`${config.formsEndpoint}`)
      .post({
        clientId: config.clientId,
        redirectUri: redirectUri,
        code: $code,
        refreshToken: $refreshToken,
      })
      .unauthorized(() => {
        continueRequest = false;
        code.set(undefined);
      })
      .json<IHubSpotFormResponse>();

    try {
      responseError = undefined;
      response = await request;
    } catch (error) {
      responseError = error;
    }

    if (!continueRequest) {
      return;
    }

    loading = false;
  };

  $: response && refreshToken.set(response.refreshToken);

  const t = translate([translations, sharedTranslations]);
</script>

<CustomElement bind:value bind:config bind:disabled>
  {#if loading}
    <Loading />
  {:else}
    <div transition:fade>
      {#if !disabled && !responseError}
        <div class="group">
          <!-- svelte-ignore a11y-no-onchange -->
          <select
            class="select"
            value={value.form && value.form.guid}
            on:change={(event) =>
              event.currentTarget.value !== "" &&
              (value.form = response.forms.find(
                (form) => form.guid === event.currentTarget.value
              ))}>
            <option value="">{$t("chooseForm")}</option>
            {#each response.forms as form (form.guid)}
              <option value={form.guid}>{form.name}</option>
            {/each}
          </select>
        </div>
      {/if}
      {#if responseError}
        <div class="group">{responseError}</div>
      {/if}
      {#if value.form}
        <div class="group" transition:fade>
          <div class="group column">
            <div>{$t("previewDescription")}</div>
            <div class="form group column">
              <h3>{value.form.name}</h3>
              {#if showForm}
                {#each value.form.formFieldGroups as formFieldGroup}
                  {#each formFieldGroup.fields as field}
                    <!-- svelte-ignore a11y-label-has-associated-control -->
                    <label class="formRow">
                      <p class="label">
                        {#if field.required}
                          {field.label}
                          *
                        {:else}{field.label}{/if}
                      </p>
                      <p class="field">
                        {#if field.fieldType === "text"}
                          <input
                            class="input"
                            name={field.name}
                            type="text"
                            placeholder={field.placeholder}
                            required={field.required}
                            value={field.defaultValue} />
                        {:else if field.fieldType === "booleancheckbox"}
                          <input
                            class="input checkbox"
                            name={field.name}
                            type="checkbox"
                            placeholder={field.placeholder}
                            required={field.required}
                            value={field.defaultValue} />
                        {:else if field.fieldType === "textarea"}
                          <textarea
                            class="input textarea"
                            name={field.name}
                            placeholder={field.placeholder}
                            required={field.required}
                            value={field.defaultValue} />
                        {/if}
                        {field.description}
                      </p>
                    </label>
                  {/each}
                {/each}
                <button class="button" on:click={() => (showForm = false)}>
                  {value.form.submitText}
                </button>
              {:else}
                <p>{value.form.inlineMessage}</p>
              {/if}
            </div>
            {#if !showForm}
              <button class="button" on:click={() => (showForm = true)}>
                {$t("reset")}
              </button>
            {/if}
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

<style>
  .form {
    width: 32em;
    border: 0.1em solid #ddd;
    display: flex;
    padding: 0.5em;
    border-radius: 0.3em;
  }

  .input {
    border: 0.1em solid #e0e0e0 !important;
    height: 2em;
    min-width: 0;
    padding: 0.25em 0.5em;
  }

  .formRow {
    display: flex;
    padding: 0 0 1em 0;
  }

  .label {
    flex: 2;
    margin: 0;
    color: initial;
    font-weight: initial;
    font-size: initial;
  }

  .field {
    flex: 10;
    margin: 0;
  }

  .checkbox {
    width: 10%;
  }

  .textarea {
    height: auto;
  }

  .button {
    width: auto;
    margin: 0 auto;
  }
</style>
