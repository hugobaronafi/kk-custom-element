using System.Collections.Generic;

using Core.HubSpot.Models;

namespace Core.HubSpot.Services
{
    public interface IHubSpotApiCache
    {
        IDictionary<string, GetTokenResponse> CodeTokenCache { get; }
    }
}