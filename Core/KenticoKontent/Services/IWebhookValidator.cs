using System;
using System.Collections.Generic;

using Core.KenticoKontent.Models.Webhook;

namespace Core.KenticoKontent.Services
{
    public interface IWebhookValidator
    {
        (bool valid, Func<Webhook> getWebhook) ValidateWebhook(string body, IDictionary<string, string> headers);
    }
}