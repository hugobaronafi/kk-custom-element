[![gallery-shield]](https://kentico.github.io/kontent-custom-element-samples/gallery/)

![last-commit]
[![issues-shield]](https://github.com/yuriys-kentico/kenticokontentkonservatory/issues)
[![forks-shield]](https://github.com/yuriys-kentico/kenticokontentkonservatory/network/members)
[![license-shield]](https://github.com/yuriys-kentico/kenticokontentkonservatory/blob/main/license)

[![stack-shield]](https://stackoverflow.com/tags/kentico-kontent)
[![discussion-shield]](https://github.com/Kentico/Home/discussions)

# Unsplash

![Element screenshot](https://assets-us-01.kc-usercontent.com/10cfe925-5d5a-0029-ac35-5fa8123935a0/c382ec86-7337-4e12-af85-33f0f15c85a8/UnsplashCustomElement.png)

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

This [custom element](https://docs.kontent.ai/tutorials/develop-apps/integrate/integrating-your-own-content-editing-features) extension for [Kentico Kontent](https://kontent.ai) is assets from Unsplash.

## Motivation

Unsplash is a popular DAM that can provide benefits to Kontent projects.

## Features

- Load one or more assets from Unsplash.
- Search for assets.

## Steps to install

1. Clone this repository:
   - Locally, then run `npm i; npm run build; npm run start`.
   - On Vercel: [![Deploy with Vercel](https://vercel.com/button)](https://vercel.com/new/git/external?repository-url=https%3A%2F%2Fgithub.com%2Fyuriys-kentico%2FKenticoKontentKonservatory%2F).
1. Follow [the steps here](https://unsplash.com/documentation#registering-your-application) to create a new application and get an access key.
1. In Kontent, choose an existing content type or create a new one by clicking `Create new`.
1. Click `Custom element` on the right to add a new element.
1. For **Hosted code URL (HTTPS)**, enter _{HOST FROM STEP 1}/elements/unsplash_.
1. For **Parameters {JSON}**, enter JSON with these required properties:
   - **accessKey** : the access key from step 2.
1. Click `Save changes` (_Ctrl + S_) and create a new item of that content type to view the element.

## Sample saved value

```json
{
    "assets": [
        {
            "id": "mJaD10XeD7w",
            "created_at": "2017-05-21T05:47:07-04:00",
            "updated_at": "2020-11-20T04:02:08-05:00",
            "promoted_at": null,
            "width": 3374,
            "height": 4498,
            "color": "#201A1D",
            "blur_hash": "LTO|96I9-;xu?wWBj?xu?vx]D%M{",
            "description": "Larry",
            "alt_description": "brown tabby cat on white stairs",
            "urls": {
                "raw": "https://images.unsplash.com/photo-1495360010541-f48722b34f7d?ixlib=rb-1.2.1&ixid=eyJhcHBfaWQiOjUzNjEwfQ",
                "full": "https://images.unsplash.com/photo-1495360010541-f48722b34f7d?ixlib=rb-1.2.1&q=85&fm=jpg&crop=entropy&cs=srgb&ixid=eyJhcHBfaWQiOjUzNjEwfQ",
                ...
            },
            "links": {
                "self": "https://api.unsplash.com/photos/mJaD10XeD7w",
                "html": "https://unsplash.com/photos/mJaD10XeD7w",
                "download": "https://unsplash.com/photos/mJaD10XeD7w/download",
                "download_location": "https://api.unsplash.com/photos/mJaD10XeD7w/download"
            },
            "categories": [],
            "likes": 191,
            "liked_by_user": false,
            "current_user_collections": [],
            "sponsorship": null,
            "user": ...,
            "tags": [
                {
                    "type": "landing_page",
                    "title": "cat",
                    "source": {
                        "ancestry": {
                            "type": {
                                "slug": "images",
                                "pretty_slug": "Images"
                            },
                            "category": {
                                "slug": "animals",
                                "pretty_slug": "Animals"
                            },
                            "subcategory": {
                                "slug": "cat",
                                "pretty_slug": "Cat"
                            }
                        },
                        "title": "Cat Images & Pictures",
                        "subtitle": "Download free cat images",
                        "description": "9 lives isn't enough to capture the amazing-ness of cats. You need high-quality, professionally photographed images to do that. Unsplash's collection of cat images capture the wonder of the kitty in high-definition, and you can use these images however you wish for free.",
                        "meta_title": "20+ Cat Pictures & Images [HD] | Download Free Images & Stock Photos on Unsplash",
                        "meta_description": "Choose from hundreds of free cat pictures. Download HD cat photos for free on Unsplash.",
                        "cover_photo": {
                            "id": "_SMNO4cN9vs",
                            "created_at": "2018-12-30T12:17:38-05:00",
                            "updated_at": "2020-10-28T01:20:32-04:00",
                            "promoted_at": "2019-01-01T05:23:58-05:00",
                            "width": 4000,
                            "height": 4000,
                            "color": "#E8C98D",
                            "blur_hash": "L012.;oL4=WBt6j@Rlay4;ay^iof",
                            "description": "Glow in the Dark",
                            "alt_description": "yellow eyes",
                            "urls": {
                                "raw": "https://images.unsplash.com/photo-1546190255-451a91afc548?ixlib=rb-1.2.1",
                                "full": "https://images.unsplash.com/photo-1546190255-451a91afc548?ixlib=rb-1.2.1&q=85&fm=jpg&crop=entropy&cs=srgb",
                                ...
                            },
                            "links": {
                                "self": "https://api.unsplash.com/photos/_SMNO4cN9vs",
                                "html": "https://unsplash.com/photos/_SMNO4cN9vs",
                                "download": "https://unsplash.com/photos/_SMNO4cN9vs/download",
                                "download_location": "https://api.unsplash.com/photos/_SMNO4cN9vs/download"
                            },
                            "categories": [],
                            "likes": 486,
                            "liked_by_user": false,
                            "current_user_collections": [],
                            "sponsorship": null,
                            "user": ...
                        }
                    }
                },
                ...
            ]
        }
    ]
}
```

## Sample config

```json
{
  "accessKey": "2e634b76600266dodo7b6aa736617aa321041624440922a36afee57ec0abc5e5"
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
