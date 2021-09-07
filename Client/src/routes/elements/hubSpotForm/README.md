[![gallery-shield]](https://kentico.github.io/kontent-custom-element-samples/gallery/)

![last-commit]
[![issues-shield]](https://github.com/yuriys-kentico/kenticokontentkonservatory/issues)
[![forks-shield]](https://github.com/yuriys-kentico/kenticokontentkonservatory/network/members)
[![license-shield]](https://github.com/yuriys-kentico/kenticokontentkonservatory/blob/main/license)

[![stack-shield]](https://stackoverflow.com/tags/kentico-kontent)
[![discussion-shield]](https://github.com/Kentico/Home/discussions)

# HubSpot form

![Element screenshot](https://assets-us-01.kc-usercontent.com/10cfe925-5d5a-0029-ac35-5fa8123935a0/fbf641f6-76ad-45bb-b36f-8e7b69782771/HubSpotFormsCustomElement.png)

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

This [custom element](https://docs.kontent.ai/tutorials/develop-apps/integrate/integrating-your-own-content-editing-features) extension for [Kentico Kontent](https://kontent.ai) is a form from HubSpot.

## Motivation

HubSpot has a popular form service that can provide benefits to Kontent projects.

## Features

- Choose any form from your HubSpot account.
- See a preview of the form and its fields.

## Steps to install

This element requires this backend function:

- [KenticoKontentKonservatory/Functions/Elements/HubspotGetForms.cs](https://github.com/yuriys-kentico/KenticoKontentKonservatory/blob/main/Functions/Elements/HubspotGetForms.cs)

1. Clone this repository:
   - Locally, then run `npm i; npm run build; npm run start`.
   - On Vercel: [![Deploy with Vercel](https://vercel.com/button)](https://vercel.com/new/git/external?repository-url=https%3A%2F%2Fgithub.com%2Fyuriys-kentico%2FKenticoKontentKonservatory%2F).
1. Follow [the steps here](https://developers.hubspot.com/docs/api/creating-an-app) to create an app and get a client secret and client ID and install it on a HubSpot account.
1. Open `Functions.sln` in Visual Studio and publish the `Functions` project to an Azure Function App with these required environment variables:
   - **HubSpot:ClientSecret** : the client secret from step 2.
1. In Kontent, choose an existing content type or create a new one by clicking `Create new`.
1. Click `Custom element` on the right to add a new element.
1. For **Hosted code URL (HTTPS)**, enter _{HOST FROM STEP 1}/elements/hubSpotForm_.
1. For **Parameters {JSON}**, enter JSON with these required properties:
   - **clientId** : the client ID from step 2.
   - **formsEndpoint** : the backend function endpoint from step 3.
1. Click `Save changes` (_Ctrl + S_) and create a new item of that content type to view the element.

## Sample saved value

```json
{
    "form": {
        "portalId": 8560707,
        "guid": "85e7c90e-7619-4456-a002-a7074a490769",
        "name": "Webinar registration form",
        "action": "",
        "method": "POST",
        "cssClass": "hs-form stacked",
        "redirect": "",
        "submitText": "Submit",
        "followUpId": "",
        "notifyRecipients": "12113889",
        "leadNurturingCampaignId": "",
        "formFieldGroups": [
            {
                "fields": [
                    {
                        "name": "company",
                        "label": "Company name",
                        "type": "string",
                        "fieldType": "text",
                        "description": "",
                        "groupName": "contactinformation",
                        "displayOrder": -1,
                        "required": false,
                        "selectedOptions": [],
                        "options": [],
                        "validation": {
                            "name": "",
                            "message": "",
                            "data": "",
                            "useDefaultBlockList": false,
                            "blockedEmailAddresses": []
                        },
                        "enabled": true,
                        "hidden": false,
                        "defaultValue": "",
                        "isSmartField": false,
                        "unselectedLabel": "",
                        "placeholder": "",
                        "dependentFieldFilters": [],
                        "labelHidden": false,
                        "propertyObjectType": "CONTACT",
                        "metaData": [],
                        "objectTypeId": "0-1"
                    }
                ],
                "default": true,
                "isSmartGroup": false,
                "richText": {
                    "content": "",
                    "type": "TEXT"
                },
                "isPageBreak": false
            },
            ...
        ],
        "createdAt": 1603124758114,
        "updatedAt": 1603124839274,
        "performableHtml": "",
        "migratedFrom": "",
        "ignoreCurrentValues": false,
        "metaData": [
            {
                "name": "lang",
                "value": "en"
            },
            {
                "name": "embedType",
                "value": "REGULAR"
            }
        ],
        "deletable": true,
        "inlineMessage": "Thanks for submitting the form.",
        "tmsId": "",
        "captchaEnabled": false,
        "campaignGuid": "",
        "cloneable": true,
        "editable": true,
        "formType": "HUBSPOT",
        "deletedAt": 0,
        "themeName": "canvas",
        "parentId": 0,
        "style": "{\"fontFamily\":\"arial, helvetica, sans-serif\",\"backgroundWidth\":\"100%\",\"labelTextColor\":\"#33475b\",\"labelTextSize\":\"13px\",\"helpTextColor\":\"#7C98B6\",\"helpTextSize\":\"11px\",\"legalConsentTextColor\":\"#33475b\",\"legalConsentTextSize\":\"14px\",\"submitColor\":\"#ff7a59\",\"submitAlignment\":\"left\",\"submitFontColor\":\"#ffffff\",\"submitSize\":\"12px\"}",
        "isPublished": true,
        "publishAt": 0,
        "unpublishAt": 0,
        "publishedAt": 0,
        "multivariateTest": {
            "variants": [],
            "startAtTimestamp": 0,
            "endAtTimestamp": 0,
            "winningVariantId": "",
            "finished": false,
            "controlId": ""
        },
        "kickbackEmailWorkflowId": 0,
        "kickbackEmailsJson": "",
        "customUid": "",
        "createMarketableContact": true,
        "editVersion": 0,
        "thankYouMessageJson": "",
        "themeColor": "",
        "alwaysCreateNewCompany": true,
        "internalUpdatedAt": 1603124839274
    }
}
```

## Sample config

```json
{
  "formsEndpoint": "https://sample.azurewebsites.net/integrations/hubSpot/getForms",
  "clientId": "11ea4520-543b-4ee8-br83-fc1ae12e1d7t"
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
