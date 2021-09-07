using System;
using System.Collections.Generic;

using Newtonsoft.Json;

namespace Core.GitHub.Models
{
    public class Tree
    {
        public string? Sha { get; set; }

        public Uri? Url { get; set; }

        [JsonProperty("tree")]
        public IList<Leaf>? TreeStructure { get; set; }
    }
}