using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;

using Core;
using Core.AzureTranslator.Models;
using Core.AzureTranslator.Services;

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace AzureTranslator.Services
{
    public class TranslationService : ITranslationService
    {
        private readonly HttpClient httpClient;

        public TranslationService(
            HttpClient httpClient,
            Settings settings
            )
        {
            this.httpClient = httpClient;

            this.httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", settings.AzureTranslator?.ApiKey);
            this.httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Region", settings.AzureTranslator?.ApiRegion);
        }

        public async Task<TranslationResult> Translate(string text, string language)
        {
            if (text.Length >= 5000)
            {
                throw new ApiException($"Text length {text.Length} is greater than 5000 and too long.");
            }

            var requestUri = $"https://api.cognitive.microsofttranslator.com/translate?api-version=3.0&textType=html&from=en&to={language}";

            var response = await PostAsJsonAsync(requestUri, new object[] { new { Text = text } });

            var content = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<TranslationResult[]>(content)[0];
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
    }
}