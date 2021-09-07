
using Newtonsoft.Json;

namespace Core.GitHub.Models
{
    public class RequiredStatusChecks
    {
        [JsonProperty("enforcement_level")]
        public string? EnforcementLevel { get; set; }

        public string[]? Contexts { get; set; }
    }
}