namespace Core.HubSpot.Models
{
    public class AuthenticatedResult : IAuthenticateResult
    {
        public string? RefreshToken { get; set; }
    }
}