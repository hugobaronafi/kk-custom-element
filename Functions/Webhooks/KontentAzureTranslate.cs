using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

using Core;
using Core.AzureTranslator.Services;
using Core.KenticoKontent.Models.Management.Elements;
using Core.KenticoKontent.Models.Management.References;
using Core.KenticoKontent.Models.Webhook;
using Core.KenticoKontent.Services;

using Functions.Functions;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace Functions.Webhooks
{
    public class KontentAzureTranslate : BaseFunction
    {
        private readonly IWebhookValidator webhookValidator;
        private readonly IKontentRepository kontentRepository;
        private readonly ITranslationService translationService;
        private readonly ITextAnalyzer textAnalyzer;

        public KontentAzureTranslate(
            ILogger<KontentAzureTranslate> logger,
            IWebhookValidator webhookValidator,
            IKontentRepository kontentRepository,
            ITranslationService translationService,
            ITextAnalyzer textAnalyzer
            ) : base(logger)
        {
            this.webhookValidator = webhookValidator;
            this.kontentRepository = kontentRepository;
            this.translationService = translationService;
            this.textAnalyzer = textAnalyzer;
        }

        [FunctionName(nameof(KontentAzureTranslate))]
        public async Task<IActionResult> Run(
            [HttpTrigger(
                "post",
                Route = Routes.KontentAzureTranslate
            )] string body,
            IDictionary<string, string> headers,
            string languageCodename
            )
        {
            try
            {
                var (valid, getWebhook) = webhookValidator.ValidateWebhook(body, headers);

                //if (!valid) return LogUnauthorized();

                var (data, message) = getWebhook();

                if (data.Items == null)
                {
                    throw new ArgumentNullException(nameof(data.Items));
                }

                switch (message.Type)
                {
                    case "content_item_variant":
                        switch (message.Operation)
                        {
                            case "change_workflow_step":
                                var translationLanguage = new CultureInfo(languageCodename).TwoLetterISOLanguageName;

                                foreach (var item in data.Items)
                                {
                                    await TranslateItem(item, languageCodename, translationLanguage);
                                }
                                break;
                        }
                        break;
                }

                return LogOk();
            }
            catch (ArgumentNullException ex)
            {
                return LogOkException(ex);
            }
            catch (ApiException ex)
            {
                return LogOkException(ex);
            }
            catch (Exception ex)
            {
                return LogException(ex);
            }
        }

        private async Task TranslateItem(Item item, string languageCodename, string translationLanguage)
        {
            if (item.ItemReference == null)
            {
                throw new ArgumentNullException(nameof(item.ItemReference));
            }

            var languageVariant = await kontentRepository.RetrieveLanguageVariant(new RetrieveLanguageVariantParameters
            {
                ItemReference = item.ItemReference,
                LanguageReference = item.LanguageReference
            });

            if (languageVariant == null)
            {
                throw new NotImplementedException("Variant could not be retrieved.");
            }

            if (languageVariant.Elements == null)
            {
                throw new NotImplementedException("Variant does not have elements.");
            }

            foreach (var element in languageVariant.Elements)
            {
                switch (element)
                {
                    case RichTextElement richTextElement:
                        {
                            var value = richTextElement.Value;

                            if (value?.Length >= 5000)
                            {
                                var parts = textAnalyzer.SplitHtml(value);
                                var longResult = "";

                                foreach (var part in parts)
                                {
                                    var (translated, translation) = await Translate(part, translationLanguage);

                                    if (translated)
                                    {
                                        longResult += translation;
                                    };
                                }

                                if (!string.IsNullOrWhiteSpace(longResult))
                                {
                                    richTextElement.Value = longResult;
                                }

                                break;
                            }

                            var result = await Translate(richTextElement.Value, translationLanguage);

                            if (result.translated)
                            {
                                richTextElement.Value = result.translation;
                            };
                            break;
                        }

                    case UrlSlugElement urlSlugElement:
                        {
                            var (translated, translation) = await Translate(urlSlugElement.Value, translationLanguage);

                            if (translated)
                            {
                                urlSlugElement.Value = translation.Replace(" ", "-");
                            };
                            break;
                        }

                    case TextElement textElement:
                        {
                            var (translated, translation) = await Translate(textElement.Value, translationLanguage);

                            if (translated)
                            {
                                textElement.Value = translation;
                            };
                            break;
                        }
                }
            }

            await kontentRepository.UpsertLanguageVariant(new UpsertLanguageVariantParameters
            {
                LanguageReference = new CodenameReference(languageCodename),
                Variant = languageVariant
            });
        }

        private async Task<(bool translated, string translation)> Translate(string? original, string translationLanguage)
        {
            if (string.IsNullOrWhiteSpace(original))
            {
                return (false, string.Empty);
            }

            var translation = (await translationService.Translate(original, translationLanguage)).Translations.First().Text;

            if (string.IsNullOrWhiteSpace(translation))
            {
                return (false, string.Empty);
            }

            return (true, translation);
        }
    }
}