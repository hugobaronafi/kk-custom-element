<script lang="ts">
  import { GraphQLClient, gql } from "graphql-request";
  import { fade } from "svelte/transition";
  import { debounce } from "lodash";

  import type { IPriceVariant, IProduct, IQueryRoot } from "./_crystallize";
  import Invalid from "../../../shared/components/customElement/invalid.svelte";
  import CustomElement from "../../../shared/components/customElement/customElement.svelte";
  import { translate } from "../../../shared/stores/translate";
  import Loading from "../../../shared/components/loading.svelte";
  import sharedTranslations from "../../../shared/components/customElement/resources";
  import type { IContext } from "../../../shared/components/customElement/customElement";
  import ObjectTile from "../../../shared/components/objectTile.svelte";
  import translations from "./_resources";

  interface ICrystallizeConfig {
    graphqlEndpoint: string;
    defaultCurrency: string;
    currencyMap: Map<string, string>;
  }

  interface ICrystallizeValue {
    product?: IProduct;
  }

  let value: ICrystallizeValue = {};
  let config: ICrystallizeConfig;
  let context: IContext;
  let disabled: boolean;

  $: graphQLClient = config && new GraphQLClient(config.graphqlEndpoint);

  let listOpen: boolean = false;
  let filter: string;
  let rawFilter: string = "";
  const debounceFilter = debounce((rawFilter) => (filter = rawFilter), 1000);

  $: rawFilter !== "" && debounceFilter(rawFilter);

  $: data =
    filter &&
    graphQLClient?.request<IQueryRoot>(gql`
    query CatalogueSearch {
      search(
        filter: {
          searchTerm: "${filter}"
          type: PRODUCT
          productVariants: { isDefault: true }
        }
        orderBy: { field: PRICE, direction: ASC }
      ) {
        edges {
          node {
            id
            name
            ... on Product {
              variants {
                sku
                priceVariants {
                  price
                  currency
                }
                images {
                  url
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
    filter = undefined;
    rawFilter = "";
  };

  const formatProductPrice = (priceVariants: IPriceVariant[]) => {
    var currencyMap = new Map(config.currencyMap);

    const { price, currency } =
      priceVariants.find(
        (priceVariant) =>
          currencyMap.get(context.variant.codename) === priceVariant.currency
      ) ||
      priceVariants.find(
        (priceVariant) => config.defaultCurrency === priceVariant.currency
      ) ||
      priceVariants[0];

    return new Intl.NumberFormat(undefined, {
      style: "currency",
      currency: currency,
    }).format(price);
  };

  const t = translate([translations, sharedTranslations]);
</script>

<CustomElement bind:value bind:config bind:context bind:disabled>
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
        {/if}
        {#if data && listOpen}
          {#await data}
            <Loading />
          {:then dataResult}
            <div class="group wrap">
              {#each dataResult.search.edges.map((edge) => edge.node) as product (product.id)}
                <ObjectTile
                  name={product.name}
                  detail={formatProductPrice(product.variants[0].priceVariants)}
                  imageUrl={product.variants[0].images[0].url}
                  thumbnailUrl={product.variants[0].images[0].url}
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
            <div class="item">
              <img
                class="image"
                src={value.product.variants[0].images[0].url}
                alt={value.product.name} />
            </div>
            <div class="description">
              <h2>{value.product.name}</h2>
              <h3>
                {formatProductPrice(value.product.variants[0].priceVariants)}
              </h3>
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
