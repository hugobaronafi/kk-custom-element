namespace Functions.Functions
{
    public static class Routes
    {
        public const string KontentAzureTranslate = "webhooks/translate/{languageCodename}";
        public const string GitHubCreatePullRequest = "webhooks/github/pullrequest/{typeCodename}/{baseName}/{repositoryName}/{headName}";
        public const string KontentGatsbyThrottle = "webhooks/gatsby/throttle/{gatsbyWebhook}";
        public const string HubSpotGetForms = "integrations/hubSpot/getForms";
        public const string KontentDeepClone = "elements/deepClone/{itemCodename}/{languageCodename}";
        public const string KontentChangeType = "elements/changeType/{itemCodename}/{languageCodename}";
        public const string KontentChangeTypeGetTypes = "elements/changeType/getTypes/{itemCodename}";
    }
}