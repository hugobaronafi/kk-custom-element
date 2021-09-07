using System;

using Newtonsoft.Json;

namespace Core.GitHub.Models
{
    public class License
    {
        public string? Key { get; set; }

        public string? Name { get; set; }

        [JsonProperty("spdx_id")]
        public string? SpdxId { get; set; }

        public Uri? Url { get; set; }

        [JsonProperty("node_id")]
        public string? NodeId { get; set; }
    }
}