[![gallery-shield]](https://kentico.github.io/kontent-custom-element-samples/gallery/)

![last-commit]
[![issues-shield]](https://github.com/yuriys-kentico/kenticokontentkonservatory/issues)
[![forks-shield]](https://github.com/yuriys-kentico/kenticokontentkonservatory/network/members)
[![license-shield]](https://github.com/yuriys-kentico/kenticokontentkonservatory/blob/main/license)

[![stack-shield]](https://stackoverflow.com/tags/kentico-kontent)
[![discussion-shield]](https://github.com/Kentico/Home/discussions)

# Font Awesome icon

![Element screenshot](https://assets-us-01.kc-usercontent.com/10cfe925-5d5a-0029-ac35-5fa8123935a0/419332c3-2301-44c8-aecb-8fbf06e53102/FontAwesomeIconCustomElement.png)

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

This [custom element](https://docs.kontent.ai/tutorials/develop-apps/integrate/integrating-your-own-content-editing-features) extension for [Kentico Kontent](https://kontent.ai) is a single icon and its metadata from Font Awesome.

## Motivation

Font Awesome contains many useful icons that can provide benefits to Kontent projects.

## Features

- Search for any icon in Font Awesome's regular set of icons.
- Stores metadata and SVG of icon.

## Steps to install

1. Clone this repository:
   - Locally, then run `npm i; npm run build; npm run start`.
   - On Vercel: [![Deploy with Vercel](https://vercel.com/button)](https://vercel.com/new/git/external?repository-url=https%3A%2F%2Fgithub.com%2Fyuriys-kentico%2FKenticoKontentKonservatory%2F).
1. In Kontent, choose an existing content type or create a new one by clicking `Create new`.
1. Click `Custom element` on the right to add a new element.
1. For **Hosted code URL (HTTPS)**, enter _{HOST FROM STEP 1}/elements/fontAwesomeIcon_.
1. For **Parameters {JSON}**, enter JSON with these required properties:
   - **iconsMetadataEndpoint** : a JSON resource that contains Font Awesome icon metadata.
1. Click `Save changes` (_Ctrl + S_) and create a new item of that content type to view the element.

## Sample saved value

```json
{
  "icon": {
    "name": "smile",
    "label": "Smiling Face",
    "unicode": "f118",
    "style": "regular",
    "svg": "<svg xmlns=\"http://www.w3.org/2000/svg\" viewBox=\"0 0 496 512\"><path d=\"M248 8C111 8 0 119 0 256s111 248 248 248 248-111 248-248S385 8 248 8zm0 448c-110.3 0-200-89.7-200-200S137.7 56 248 56s200 89.7 200 200-89.7 200-200 200zm-80-216c17.7 0 32-14.3 32-32s-14.3-32-32-32-32 14.3-32 32 14.3 32 32 32zm160 0c17.7 0 32-14.3 32-32s-14.3-32-32-32-32 14.3-32 32 14.3 32 32 32zm4 72.6c-20.8 25-51.5 39.4-84 39.4s-63.2-14.3-84-39.4c-8.5-10.2-23.7-11.5-33.8-3.1-10.2 8.5-11.5 23.6-3.1 33.8 30 36 74.1 56.6 120.9 56.6s90.9-20.6 120.9-56.6c8.5-10.2 7.1-25.3-3.1-33.8-10.1-8.4-25.3-7.1-33.8 3.1z\"/></svg>",
    "cssClass": "far fa-smile"
  }
}
```

## Sample config

```json
{
  "iconsMetadataEndpoint": "https://raw.githubusercontent.com/FortAwesome/Font-Awesome/master/metadata/icons.json"
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
