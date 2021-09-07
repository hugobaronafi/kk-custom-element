[![gallery-shield]](https://kentico.github.io/kontent-custom-element-samples/gallery/)

![last-commit]
[![issues-shield]](https://github.com/yuriys-kentico/kenticokontentkonservatory/issues)
[![forks-shield]](https://github.com/yuriys-kentico/kenticokontentkonservatory/network/members)
[![license-shield]](https://github.com/yuriys-kentico/kenticokontentkonservatory/blob/main/license)

[![stack-shield]](https://stackoverflow.com/tags/kentico-kontent)
[![discussion-shield]](https://github.com/Kentico/Home/discussions)

# Crystallize

![Element screenshot](https://assets-us-01.kc-usercontent.com/10cfe925-5d5a-0029-ac35-5fa8123935a0/297e8f24-f263-4db4-87e0-1ff226dca5d9/CrystallizeCustomElement.png)

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

This [custom element](https://docs.kontent.ai/tutorials/develop-apps/integrate/integrating-your-own-content-editing-features) extension for [Kentico Kontent](https://kontent.ai) is a product from Crystallize.

## Motivation

Crystallize is a popular PIM that can provide benefits to Kontent projects.

## Features

- Search for any product from your Crystallize store.
- Localize currencies based on the item's languages.
- Preview useful content from the chosen product.

## Steps to install

1. Clone this repository:
   - Locally, then run `npm i; npm run build; npm run start`.
   - On Vercel: [![Deploy with Vercel](https://vercel.com/button)](https://vercel.com/new/git/external?repository-url=https%3A%2F%2Fgithub.com%2Fyuriys-kentico%2FKenticoKontentKonservatory%2F).
1. In Kontent, choose an existing content type or create a new one by clicking `Create new`.
1. Click `Custom element` on the right to add a new element.
1. For **Hosted code URL (HTTPS)**, enter _{HOST FROM STEP 1}/elements/crystallize_.
1. For **Parameters {JSON}**, enter JSON with these required properties:
   - **graphqlEndpoint** : [the GraphQL Search API endpoint](https://crystallize.com/learn/developer-guides/api-overview/api-endpoints) for your Crystallize store.
1. You can also add these optional properties:
   - **currencyMap** : [a Map](https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/Map) of project language codenames to currency codes.
   - **defaultCurrency** : the default currency to use when there is no match in the Map or no Map.
1. Click `Save changes` (_Ctrl + S_) and create a new item of that content type to view the element.

## Sample saved value

```json
{
  "product": {
    "id": "5faa875de8f6e2433cf877c5",
    "name": "Chemex",
    "variants": [
      {
        "sku": "your-sku-1604011292891",
        "priceVariants": [
          {
            "price": 45.99,
            "currency": "eur"
          },
          {
            "price": 49.99,
            "currency": "usd"
          }
        ],
        "images": [
          {
            "url": "https://media.crystallize.com/kentico-kontent/20/11/10/2/ebcdae2cb9029cd541577e49cc234725f57ae5ef.jpg"
          }
        ]
      }
    ]
  }
}
```

## Sample config

```json
{
  "graphqlEndpoint": "https://api.crystallize.com/sample/search",
  "currencyMap": [
    ["en-US", "usd"],
    ["es-ES", "eur"]
  ],
  "defaultCurrency": "usd"
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
