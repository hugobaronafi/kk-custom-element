using System;

using Newtonsoft.Json;

namespace Core.GitHub.Models
{
    public class Commit
    {
        public string? Sha { get; set; }

        [JsonProperty("node_id")]
        public string? NodeId { get; set; }

        [JsonProperty("commit")]
        public CommitContext? CommitContext { get; set; }

        public Uri? Url { get; set; }

        [JsonProperty("html_url")]
        public Uri? HtmlUrl { get; set; }

        [JsonProperty("comments_url")]
        public Uri? CommentsUrl { get; set; }

        public User? Author { get; set; }

        public User? Committer { get; set; }

        public Commit[]? Parents { get; set; }
    }
}