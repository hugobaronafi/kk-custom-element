using Newtonsoft.Json;

namespace Core.GitHub.Models
{
    public class RichLinks
    {
        [JsonProperty("self")]
        public RichLink? Self { get; set; }

        [JsonProperty("html")]
        public RichLink? Html { get; set; }

        [JsonProperty("issue")]
        public RichLink? Issue { get; set; }

        [JsonProperty("comments")]
        public RichLink? Comments { get; set; }

        [JsonProperty("review_comments")]
        public RichLink? ReviewComments { get; set; }

        [JsonProperty("review_comment")]
        public RichLink? ReviewComment { get; set; }

        [JsonProperty("commits")]
        public RichLink? Commits { get; set; }

        [JsonProperty("statuses")]
        public RichLink? Statuses { get; set; }
    }
}