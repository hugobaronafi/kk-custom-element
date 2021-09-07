[![gallery-shield]](https://kentico.github.io/kontent-custom-element-samples/gallery/)

![last-commit]
[![issues-shield]](https://github.com/yuriys-kentico/kenticokontentkonservatory/issues)
[![forks-shield]](https://github.com/yuriys-kentico/kenticokontentkonservatory/network/members)
[![license-shield]](https://github.com/yuriys-kentico/kenticokontentkonservatory/blob/main/license)

[![stack-shield]](https://stackoverflow.com/tags/kentico-kontent)
[![discussion-shield]](https://github.com/Kentico/Home/discussions)

# Bynder

![Element screenshot](https://assets-us-01.kc-usercontent.com/10cfe925-5d5a-0029-ac35-5fa8123935a0/199d3b0a-7b2a-4690-be1c-f2a1dc6b37ad/BynderCustomElement.png)

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

This [custom element](https://docs.kontent.ai/tutorials/develop-apps/integrate/integrating-your-own-content-editing-features) extension for [Kentico Kontent](https://kontent.ai) is assets from Bynder.

## Motivation

Bynder is a popular DAM that can provide benefits to Kontent projects.

## Features

- Load one or more assets from any Bynder repository.
- Search for assets.

## Steps to install

1. Clone this repository:
   - Locally, then run `npm i; npm run build; npm run start`.
   - On Vercel: [![Deploy with Vercel](https://vercel.com/button)](https://vercel.com/new/git/external?repository-url=https%3A%2F%2Fgithub.com%2Fyuriys-kentico%2FKenticoKontentKonservatory%2F).
1. In Kontent, choose an existing content type or create a new one by clicking `Create new`.
1. Click `Custom element` on the right to add a new element.
1. For **Hosted code URL (HTTPS)**, enter _{HOST FROM STEP 1}/elements/bynder_.
1. For **Parameters {JSON}**, enter JSON with a property named `bynderOptions` with these required properties:
   - **defaultDomain** : the Bynder domain of your store.
1. You can also add to that property [the optional object and scalar properties in the table here](https://developer-docs.bynder.com/ui-components/).
1. Click `Save changes` (_Ctrl + S_) and create a new item of that content type to view the element.

## Sample saved value

```json
{
    "assets": [
        {
            "__typename": "Image",
            "id": "KEFzc2V0X2lkIDU2QTk4OURCLTNFMjctNDQwMy05ODc5MEI2QTQ3MDM2NENFKQ==",
            "name": "Logo-misuse-6@2x",
            "description": null,
            "databaseId": "56A989DB-3E27-4403-98790B6A470364CE",
            "createdAt": "2019-10-21T08:40:28Z",
            "originalUrl": null,
            "publishedAt": "2019-10-21T08:37:11Z",
            "tags": [
                "guidelines"
            ],
            "type": "IMAGE",
            "updatedAt": "2019-10-21T08:40:28Z",
            "url": "https://wave-trial.getbynder.com/media/?mediaId=56A989DB-3E27-4403-98790B6A470364CE",
            "metaproperties": {
                "nodes": [
                    {
                        "name": "AssetType",
                        "type": "select2",
                        "options": [
                            {
                                "name": "Graphic",
                                "displayLabel": "Graphic/Visual"
                            }
                        ]
                    },
                   ...
                ]
            },
            "textMetaproperties": [],
            "derivatives": {
                "webImage": "https://d2csxpduxe849s.cloudfront.net/media/C63D0E08-B7D2-4B08-ABB54052F41999E0/56A989DB-3E27-4403-98790B6A470364CE/webimage-E42AB573-1DF8-405B-9179627D8C4D35A3.jpg",
                ...
            },
            "files": {
                "webImage": {
                    "url": "https://d2csxpduxe849s.cloudfront.net/media/C63D0E08-B7D2-4B08-ABB54052F41999E0/56A989DB-3E27-4403-98790B6A470364CE/webimage-E42AB573-1DF8-405B-9179627D8C4D35A3.jpg",
                    "width": 800,
                    "height": 454,
                    "fileSize": null
                },
               ...
            }
        }
    ]
}
```

## Sample config

```json
{
  "bynderOptions": {
    "defaultDomain": "https://sample.bynder.com/",
    "assetTypes": ["image"]
  }
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
