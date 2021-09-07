[![gallery-shield]](https://kentico.github.io/kontent-custom-element-samples/gallery/)

![last-commit]
[![issues-shield]](https://github.com/yuriys-kentico/kenticokontentkonservatory/issues)
[![forks-shield]](https://github.com/yuriys-kentico/kenticokontentkonservatory/network/members)
[![license-shield]](https://github.com/yuriys-kentico/kenticokontentkonservatory/blob/main/license)

[![stack-shield]](https://stackoverflow.com/tags/kentico-kontent)
[![discussion-shield]](https://github.com/Kentico/Home/discussions)

# Change type

![Element screenshot](https://assets-us-01.kc-usercontent.com/10cfe925-5d5a-0029-ac35-5fa8123935a0/8434c5fb-cceb-4a0e-852d-80320d39a432/ChangeTypeCustomElement.png)

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

This [custom element](https://docs.kontent.ai/tutorials/develop-apps/integrate/integrating-your-own-content-editing-features) extension for [Kentico Kontent](https://kontent.ai) allows editors to change the type of the item by mapping its content to a new item of the target type.

## Motivation

Sometimes a Kontent model is mature and has a lot of content, but some content needs to change to another type.

## Features

- Change from the current item's type to any other type in the same project.
- Map elements to similar elements.
- New item is created in _Draft_.
- Update all links to this item to the new item.
- Current item is unchanged.

## Steps to install

This element requires these backend functions:

- [KenticoKontentKonservatory/Functions/Elements/KontentChangeType.cs](https://github.com/yuriys-kentico/KenticoKontentKonservatory/blob/main/Functions/Elements/KontentChangeType.cs)
- [KenticoKontentKonservatory/Functions/Elements/KontentChangeTypeGetTypes.cs](https://github.com/yuriys-kentico/KenticoKontentKonservatory/blob/main/Functions/Elements/KontentChangeTypeGetTypes.cs)

1. Clone this repository:
   - Locally, then run `npm i; npm run build; npm run start`.
   - On Vercel: [![Deploy with Vercel](https://vercel.com/button)](https://vercel.com/new/git/external?repository-url=https%3A%2F%2Fgithub.com%2Fyuriys-kentico%2FKenticoKontentKonservatory%2F).
1. Open `Functions.sln` in Visual Studio and publish the `Functions` project to an Azure Function App with these required environment variables:
   - **KenticoKontent:ProjectId** : the project ID.
   - **KenticoKontent:ManagementApiKey** : the project's Management API key.
1. In Kontent, choose an existing content type or create a new one by clicking `Create new`.
1. Click `Custom element` on the right to add a new element.
1. For **Hosted code URL (HTTPS)**, enter _{HOST FROM STEP 1}/elements/changeType_.
1. For **Parameters {JSON}**, enter JSON with these required properties:
   - **changeTypeEndpoint** : the first backend function endpoint from step 1.
   - **getTypesEndpoint** : the second backend function endpoint from step 1.
1. Click `Save changes` (_Ctrl + S_) and create a new item of that content type to view the element.

## Sample saved value

```json
null
```

## Sample config

```json
{
  "changeTypeEndpoint": "https://sample.azurewebsites.net/elements/changeType",
  "getTypesEndpoint": "https://sample.azurewebsites.net/elements/changeType/getTypes"
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
