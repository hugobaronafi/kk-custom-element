[![gallery-shield]](https://kentico.github.io/kontent-custom-element-samples/gallery/)

![last-commit]
[![issues-shield]](https://github.com/yuriys-kentico/kenticokontentkonservatory/issues)
[![forks-shield]](https://github.com/yuriys-kentico/kenticokontentkonservatory/network/members)
[![license-shield]](https://github.com/yuriys-kentico/kenticokontentkonservatory/blob/main/license)

[![stack-shield]](https://stackoverflow.com/tags/kentico-kontent)
[![discussion-shield]](https://github.com/Kentico/Home/discussions)

# Shopify

![Element screenshot](https://assets-us-01.kc-usercontent.com/10cfe925-5d5a-0029-ac35-5fa8123935a0/253d15d9-8e8e-485f-ac1b-c0088b5956ab/ShopifyCustomElement.png)

<p align="center">
  <a href="#description">Description</a> •
  <a href="#motivation">Motivation</a> •
  <a href="#features">Features</a> •
  <a href="#steps-to-install">Install</a> •
  <a href="#sample-saved-value">Value</a> •
  <a href="#sample-config">Config</a> •
  <a href="#developing">Developing</a> •
  <a href="#license">License</a> •
  <a href="#additional-resources">Resources</a>
</p>

## Description

This [custom element](https://docs.kontent.ai/tutorials/develop-apps/integrate/integrating-your-own-content-editing-features) extension for [Kentico Kontent](https://kontent.ai) is a product from Shopify.

## Motivation

Shopify is a popular PIM that can provide benefits to Kontent projects.

## Features

- Search for any product from your Shopify store.
- Preview useful content from the chosen product.

## Steps to install

1. Clone this repository:
   - Locally, then run `npm i; npm run build; npm run start`.
   - On Vercel: [![Deploy with Vercel](https://vercel.com/button)](https://vercel.com/new/git/external?repository-url=https%3A%2F%2Fgithub.com%2Fyuriys-kentico%2FKenticoKontentKonservatory%2F).
1. Follow [the steps here](https://shopify.dev/docs/storefront-api/getting-started#private-app) to create a private Shopify app, enable the Storefront API, and get a storefront access token.
1. In Kontent, choose an existing content type or create a new one by clicking `Create new`.
1. Click `Custom element` on the right to add a new element.
1. For **Hosted code URL (HTTPS)**, enter _{HOST FROM STEP 1}/elements/shopify_.
1. For **Parameters {JSON}**, enter JSON with these required properties:
   - **storefrontAccessToken** : the storefront access token from step 2.
   - **graphqlEndpoint** : a GraphQL endpoint for your Shopify store and [the version of the API](https://shopify.dev/concepts/about-apis/versioning#calling-an-api-version) you want to use.
1. Click `Save changes` (_Ctrl + S_) and create a new item of that content type to view the element.

## Sample saved value

```json
{
  "product": {
    "id": "Z2lkOi8vc2hvcGlmeS9Qcm9kdWN0LzU3NTg0Mzg4MzQzMzM=",
    "images": {
      "edges": [
        {
          "node": {
            "originalSrc": "https://cdn.shopify.com/s/files/1/0496/7320/7965/products/chemex_x250.jpg?v=1603204718",
            "transformedSrc": "https://cdn.shopify.com/s/files/1/0496/7320/7965/products/chemex.jpg?v=1603204718"
          }
        }
      ]
    },
    "title": "Chemex",
    "descriptionHtml": "<p data-mce-fragment=\"1\">A coffee brewer that can raise the spirit of your home as well? Of course, the Chemex, with its unique hand-fitting shape, wrapped in a polished wooden collar and a leather tie.<br data-mce-fragment=\"1\"> <br data-mce-fragment=\"1\"> Using Chemex, you will produce a full-flavoured brew at the strength of your liking. The glass is heat-resistant, so it's safe and you can use its handle to manipulate the device safely. The filters (sold separately) are natural and unbleached and deliver a taste without bitterness or sediment.<img src=\"#\" data-asset-id=\"83d110b2-3bff-4b4e-9070-f6b30e7802ea\" data-mce-fragment=\"1\" data-mce-src=\"#\"></p>\n<p data-mce-fragment=\"1\">You'll appreciate its elegance, proven by its forty-year tradition. This remains to this day one of the truly chic ways of making coffee. Made in the USA.<br data-mce-fragment=\"1\"> <br data-mce-fragment=\"1\"> All you need do is insert a filter and beans and pour freshly hot water over them. </p>",
    "variants": {
      "edges": [
        {
          "node": {
            "id": "Z2lkOi8vc2hvcGlmeS9Qcm9kdWN0VmFyaWFudC8zNjc2NDEyMzAwNTA4NQ==",
            "priceV2": {
              "amount": "49.9",
              "currencyCode": "USD"
            }
          }
        }
      ]
    }
  }
}
```

## Sample config

```json
{
  "storefrontAccessToken": "0abcd612fc55291a4af0c123c529ef90",
  "graphqlEndpoint": "https://sample.myshopify.com/api/2020-10/graphql"
}
```

## Developing

```bash
$ npm i; npm run dev
```

## License

[MIT](https://tldrlegal.com/license/mit-license)

## Additional Resources

- [Custom Element Gallery on GitHub](https://kentico.github.io/kontent-custom-element-samples/gallery/)
- [Integrations documentation](https://docs.kontent.ai/tutorials/develop-apps/integrate/integrations-overview)
- [Custom element documentation](https://docs.kontent.ai/tutorials/develop-apps/integrate/content-editing-extensions)
- [Custom element API reference](https://docs.kontent.ai/reference/custom-elements-js-api)

[gallery-shield]: https://img.shields.io/static/v1?label=&message=extension%20gallery&color=51bce0&style=for-the-badge
[last-commit]: https://img.shields.io/github/last-commit/yuriys-kentico/KenticoKontentKonservatory?style=for-the-badge
[issues-shield]: https://img.shields.io/github/issues/yuriys-kentico/KenticoKontentKonservatory.svg?style=for-the-badge
[forks-shield]: https://img.shields.io/github/forks/yuriys-kentico/KenticoKontentKonservatory.svg?style=for-the-badge
[license-shield]: https://img.shields.io/github/license/yuriys-kentico/KenticoKontentKonservatory.svg?style=for-the-badge
[stack-shield]: https://img.shields.io/badge/Stack%20Overflow-ASK%20NOW-FE7A16.svg?logo=stackoverflow&logoColor=white&style=for-the-badge
[discussion-shield]: https://img.shields.io/badge/GitHub-Discussions-FE7A16.svg?logo=github&style=for-the-badge
