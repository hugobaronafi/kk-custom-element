using System;

using Newtonsoft.Json;

namespace Core.GitHub.Models
{
    public class Branch
    {
        public string? Name { get; set; }

        public Commit? Commit { get; set; }

        [JsonProperty("_links")]
        public Links? Links { get; set; }

        [JsonProperty("protected")]
        public bool? Protected { get; set; }

        public Protection? Protection { get; set; }

        [JsonProperty("protection_url")]
        public Uri? ProtectionUrl { get; set; }
    }
}