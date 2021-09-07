using Newtonsoft.Json;

namespace Core.GitHub.Models
{
    public class Protection
    {
        public bool? Enabled { get; set; }

        [JsonProperty("required_status_checks")]
        public RequiredStatusChecks? RequiredStatusChecks { get; set; }
    }
}