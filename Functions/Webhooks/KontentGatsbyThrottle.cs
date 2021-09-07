using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;

using Azure.Storage.Queues;

using Core;
using Core.Gatsby.Models;
using Core.Gatsby.Services;
using Core.KenticoKontent.Services;

using Functions.Functions;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

using Newtonsoft.Json;

namespace Functions.Webhooks
{
    public class KontentGatsbyThrottle : BaseFunction
    {
        private readonly QueueClient queueClient;
        private readonly IWebhookValidator webhookValidator;
        private readonly IGatsbyProxy gatsbyProxy;

        private readonly Action<DebouncedItem> debounced;

        public KontentGatsbyThrottle(
            ILogger<KontentAzureTranslate> logger,
            QueueClient queueClient,
            IWebhookValidator webhookValidator,
            Action<DebouncedItem> debounced,
            IGatsbyProxy gatsbyProxy
            ) : base(logger)
        {
            this.queueClient = queueClient;
            this.webhookValidator = webhookValidator;
            this.gatsbyProxy = gatsbyProxy;
            this.debounced = debounced;
        }

        [FunctionName(nameof(KontentGatsbyThrottle))]
        public async Task<IActionResult> Run(
            [HttpTrigger(
                "post",
                Route = Routes.KontentGatsbyThrottle
            )] string body,
            IDictionary<string, string> headers,
            string gatsbyWebhook
            )
        {
            try
            {
                var (valid, getWebhook) = webhookValidator.ValidateWebhook(body, headers);

                //if (!valid) return LogUnauthorized();

                var webhook = getWebhook();
                var (data, message) = webhook;

                switch (message.Type)
                {
                    case "content_item_variant":
                        switch (message.Operation)
                        {
                            default:
                                var parsedGatsbyWebhook = HttpUtility.UrlDecode(HttpUtility.UrlDecode(gatsbyWebhook));

                                queueClient.SendMessage(JsonConvert.SerializeObject(new QueueItem(webhook, parsedGatsbyWebhook)));
                                debounced(new DebouncedItem(webhook, parsedGatsbyWebhook));

                                // timer checks queue, sends latest
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
    }
}