using System;

namespace Core.GitHub.Models
{
    public class Leaf
    {
        public string? Path { get; set; }

        public string? Mode { get; set; }

        public string? Type { get; set; }

        public string? Sha { get; set; }

        public int? Size { get; set; }

        public Uri? Url { get; set; }
    }
}