using Newtonsoft.Json;

using System;

namespace Core.GitHub.Models
{
    public class Team
    {
        public string? Name { get; set; }

        public long? Id { get; set; }

        [JsonProperty("node_id")]
        public string? NodeId { get; set; }

        public string? Slug { get; set; }

        public string? Description { get; set; }

        public string? Privacy { get; set; }

        public Uri? Url { get; set; }

        [JsonProperty("html_url")]
        public Uri? HtmlUrl { get; set; }

        [JsonProperty("members_url")]
        public string? MembersUrl { get; set; }

        [JsonProperty("repositories_url")]
        public Uri? RepositoriesUrl { get; set; }

        [JsonProperty("permission")]
        public string? Permission { get; set; }

        public Team? Parent { get; set; }
    }
}