<script lang="ts">
  import { GraphQLClient, gql } from "graphql-request";
  import DOMPurify from "dompurify";
  import { fade } from "svelte/transition";

  import type { IPriceV2, IProduct, IQueryRoot } from "./_shopify";
  import Invalid from "../../../shared/components/customElement/invalid.svelte";
  import CustomElement from "../../../shared/components/customElement/customElement.svelte";
  import { translate } from "../../../shared/stores/translate";
  import Loading from "../../../shared/components/loading.svelte";
  import sharedTranslations from "../../../shared/components/customElement/resources";
  import ObjectTile from "../../../shared/components/objectTile.svelte";
  import translations from "./_resources";

  interface IShopifyConfig {
    storefrontAccessToken: string;
    graphqlEndpoint: string;
  }

  interface IShopifyValue {
    product?: IProduct;
  }

  let value: IShopifyValue = {};
  let config: IShopifyConfig;
  let disabled: boolean;

  let listOpen: boolean = false;
  let filter: string = "";

  $: graphQLClient =
    config &&
    new GraphQLClient(config.graphqlEndpoint, {
      headers: {
        "X-Shopify-Storefront-Access-Token": config.storefrontAccessToken,
        accept: "application/json",
      },
    });

  $: data = graphQLClient?.request<IQueryRoot>(gql`
    {
      products(first: 250) {
        edges {
          node {
            id
            images(first: 1, maxHeight: 250) {
              edges {
                node {
                  originalSrc
                  transformedSrc
                }
              }
            }
            title
            descriptionHtml
            variants(first: 1) {
              edges {
                node {
                  id
                  priceV2 {
                    amount
                    currencyCode
                  }
                }
              }
            }
          }
        }
      }
    }
  `);

  const closeList = () => {
    listOpen = false;
    filter = "";
  };

  const filterData = (result: IQueryRoot) =>
    result.products.edges
      .map((edge) => edge.node)
      .filter((product) => {
        if (!filter) {
          return true;
        }

        const matches = (value: string) =>
          value.match(new RegExp(filter, "gi"));

        if (matches(product.title)) {
          return true;
        }

        if (matches(product.descriptionHtml)) {
          return true;
        }

        return false;
      });

  const formatProductPrice = (price: IPriceV2) => {
    const { amount, currencyCode } = price;

    return new Intl.NumberFormat(undefined, {
      style: "currency",
      currency: currencyCode,
    }).format(parseFloat(amount));
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
        {#if value.product}
          <button
            class="button destructive"
            in:fade|local
            on:click={() => (value.product = undefined)}>
            {$t("clear")}
          </button>
        {/if}
      </div>
      <div class="group column">
        {#if listOpen}
          {#await data}
            <Loading />
          {:then result}
            <div class="group" transition:fade>
              <label class="group column filter"
                ><div class="label">
                  {$t("search")}
                </div>
                <input
                  class="input"
                  type="text"
                  placeholder={$t("placeholder")}
                  bind:value={filter} />
              </label>
            </div>
            <div class="group wrap">
              {#each filterData(result) as product (product.id)}
                <ObjectTile
                  name={product.title}
                  detail={formatProductPrice(
                    product.variants.edges[0].node.priceV2
                  )}
                  imageUrl={product.images.edges[0].node.originalSrc}
                  thumbnailUrl={product.images.edges[0].node.originalSrc}
                  onClick={() => {
                    value.product = product;
                    closeList();
                  }} />
              {/each}
            </div>
          {/await}
        {/if}
      </div>
    {/if}
    {#if value.product}
      <div class="group" transition:fade>
        <div class="group column">
          <div>{$t("previewDescription")}</div>
          <div class="group">
            <div>
              <img
                class="image"
                src={value.product.images.edges[0].node.originalSrc}
                alt={value.product.title} />
            </div>
            <div class="description">
              <h2>{value.product.title}</h2>
              <h3>
                {formatProductPrice(
                  value.product.variants.edges[0].node.priceV2
                )}
              </h3>
              {@html DOMPurify.sanitize(value.product.descriptionHtml, {
                WHOLE_DOCUMENT: false,
                FORBID_TAGS: ["object"],
              })}
            </div>
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
  .image {
    max-width: 100%;
  }

  .description {
    flex: 2;
    margin: 0 1em;
  }

  .filter {
    flex: 1;
  }
</style>
