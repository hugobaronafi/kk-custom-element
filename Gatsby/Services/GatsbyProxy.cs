using System;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading;
using System.Threading.Tasks;

using Core.Gatsby.Models;
using Core.Gatsby.Services;
using Core.KenticoKontent.Models.Webhook;

using Newtonsoft.Json.Serialization;

namespace Gatsby.Services
{
    public class GatsbyProxy : IGatsbyProxy
    {
        private readonly HttpClient httpClient;

        public GatsbyProxy(
            HttpClient httpClient
            )
        {
            this.httpClient = httpClient;
        }

        public async Task ForwardWebhook(Webhook webhook, string gatsbyWebhook)
        {
            await PostAsJsonAsync(gatsbyWebhook, webhook);
        }

        private async Task<HttpResponseMessage> PostAsJsonAsync(string requestUri, object value)
        {
            var response = await httpClient.PostAsync(requestUri, value, new JsonMediaTypeFormatter()
            {
                SerializerSettings =
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                }
            });

            return response;
        }

        public Action<T1> Debounce<T1>(Action<DebounceMode, T1> function, TimeSpan wait, DebounceMode mode = DebounceMode.Trailing)
        {
            var cancellationTokenSource = new CancellationTokenSource();
            var leadCancellationTokenSource = new CancellationTokenSource();
            var leadCancelled = false;

            var lockObject = new object();
            var leadLockObject = new object();

            return (t1) =>
            {
                lock (lockObject)
                {
                    cancellationTokenSource.Cancel();
                    cancellationTokenSource = new CancellationTokenSource();
                }

                Task.Delay(wait, cancellationTokenSource.Token)
                    .ContinueWith(task =>
                    {
                        if (mode > DebounceMode.Leading && !task.IsCanceled)
                        {
                            leadCancelled = false;
                            function(DebounceMode.Trailing, t1);
                        }
                    });

                if (mode < DebounceMode.Trailing && !leadCancelled)
                {
                    leadCancelled = true;

                    Task.Delay(wait, leadCancellationTokenSource.Token)
                        .ContinueWith(task =>
                        {
                            if (!task.IsCanceled)
                            {
                                lock (leadLockObject)
                                {
                                    leadCancellationTokenSource.Cancel();
                                    leadCancellationTokenSource = new CancellationTokenSource();
                                }
                            }
                        });

                    function(DebounceMode.Leading, t1);
                }
            };
        }
    }
}