using System.Collections.Generic;

using Core.HubSpot.Models;
using Core.HubSpot.Services;

namespace HubSpot.Services
{
    public class HubSpotApiCache : IHubSpotApiCache
    {
        public IDictionary<string, GetTokenResponse> CodeTokenCache { get; } = new Dictionary<string, GetTokenResponse>();
    }
}