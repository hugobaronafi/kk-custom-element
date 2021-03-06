namespace Core.GitHub.Models
{
    public class PullRequestContext
    {
        public string? Label { get; set; }

        public string? Ref { get; set; }

        public string? Sha { get; set; }

        public User? User { get; set; }

        public Repository? Repo { get; set; }
    }
}