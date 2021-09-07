using System;

namespace Core
{
    public class Settings
    {
        public KenticoKontentSettings? KenticoKontent { get; set; }

        public HubSpotSettings? HubSpot { get; set; }

        public AzureTranslatorSettings? AzureTranslator { get; set; }

        public GatsbySettings? Gatsby { get; set; }

        public GitHubSettings? GitHub { get; set; }
    }

    public class KenticoKontentSettings
    {
        public Guid ProjectId { get; set; }

        public string ManagementApiKey { get; set; } = string.Empty;

        public string WebhookSecret { get; set; } = string.Empty;
    }

    public class HubSpotSettings
    {
        public string ClientSecret { get; set; } = string.Empty;
    }

    public class AzureTranslatorSettings
    {
        public string ApiKey { get; set; } = string.Empty;

        public string ApiRegion { get; set; } = string.Empty;
    }

    public class GatsbySettings
    {
        public string QueueConnectionString { get; set; } = string.Empty;

        public string QueueName { get; set; } = string.Empty;

        public int DebounceSeconds { get; set; }
    }

    public class GitHubSettings
    {
        public string AuthorizationToken { get; set; } = string.Empty;

        public string UserAgent { get; set; } = string.Empty;
    }
}