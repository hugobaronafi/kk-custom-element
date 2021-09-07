using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

using Core;
using Core.HubSpot.Models;
using Core.HubSpot.Services;

namespace HubSpot.Services
{
    public class HubSpotRepository : IHubSpotRepository
    {
        private readonly HttpClient httpClient;
        private readonly Settings settings;
        private readonly IHubSpotApiCache hubSpotApiCache;

        public HubSpotRepository(
            HttpClient httpClient,
            Settings settings,
            IHubSpotApiCache hubSpotApiCache
            )
        {
            this.httpClient = httpClient;
            this.settings = settings;
            this.hubSpotApiCache = hubSpotApiCache;
        }

        public async Task<IAuthenticateResult> Authenticate(AuthenticateParameters authenticateParameters)
        {
            var (clientId, redirectUri, code, refreshToken) = authenticateParameters;

            GetTokenResponse? response;

            if (!string.IsNullOrWhiteSpace(refreshToken))
            {
                var formData = new Dictionary<string, string?>
                    {
                        { "grant_type",  "refresh_token" },
                        { "client_id", clientId },
                        { "client_secret", settings.HubSpot?.ClientSecret },
                        { "redirect_uri", redirectUri },
                        { "refresh_token", refreshToken },
                    };

                var request = await httpClient.PostAsync("https://api.hubapi.com/oauth/v1/token", new FormUrlEncodedContent(formData));

                response = await request.Content.ReadAsAsync<GetTokenResponse>();

                hubSpotApiCache.CodeTokenCache.TryAdd(code, response);
            }

            if (!hubSpotApiCache.CodeTokenCache.TryGetValue(code, out response))
            {
                var formData = new Dictionary<string, string?>
                {
                    { "grant_type",  "authorization_code" },
                    { "client_id", clientId },
                    { "client_secret", settings.HubSpot?.ClientSecret },
                    { "redirect_uri", redirectUri },
                    { "code", code },
                };

                var request = await httpClient.PostAsync("https://api.hubapi.com/oauth/v1/token", new FormUrlEncodedContent(formData));

                if (request.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    return new NotAuthenticatedResult();
                }

                response = await request.Content.ReadAsAsync<GetTokenResponse>();

                hubSpotApiCache.CodeTokenCache.TryAdd(code, response);
            }

            if (response != null)
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", response.Access_token);

                return new AuthenticatedResult
                {
                    RefreshToken = response.Refresh_token
                };
            }

            throw new NotImplementedException();
        }

        public async Task<IEnumerable<dynamic>> GetForms()
        {
            var request = await httpClient.GetAsync("https://api.hubapi.com/forms/v2/forms");

            return await request.Content.ReadAsAsync<IEnumerable<dynamic>>();
        }
    }
}