using System;

namespace Core.HubSpot.Services
{
    public class AuthenticateParameters
    {
        public string? ClientId { get; set; }

        public string? RedirectUri { get; set; }

        public string? Code { get; set; }

        public string? RefreshToken { get; set; }

        public void Deconstruct(
            out string clientId,
            out string redirectUri,
            out string code,
            out string? refreshToken
            )
        {
            clientId = ClientId ?? throw new ArgumentNullException(nameof(ClientId));
            redirectUri = RedirectUri ?? throw new ArgumentNullException(nameof(RedirectUri));
            code = Code ?? throw new ArgumentNullException(nameof(Code));
            refreshToken = RefreshToken;
        }
    }
}