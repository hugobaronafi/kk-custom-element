using System;

using Newtonsoft.Json;

namespace Core.GitHub.Models
{
    public class CommitContext
    {
        public Person? Author { get; set; }

        public Person? Committer { get; set; }

        public string? Message { get; set; }

        public Tree? Tree { get; set; }

        public Uri? Url { get; set; }

        [JsonProperty("comment_count")]
        public int? CommentCount { get; set; }

        public Verification? Verification { get; set; }
    }
}