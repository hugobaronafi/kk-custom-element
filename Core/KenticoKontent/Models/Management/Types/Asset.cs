using System.Collections.Generic;

using Newtonsoft.Json;

namespace Core.KenticoKontent.Models.Management.Types
{
    public class Asset
    {
        public string? Id { get; set; }

        [JsonProperty("file_name")]
        public string? FileName { get; set; }

        public string? Url { get; set; }
    }
}