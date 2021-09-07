using System;

using Newtonsoft.Json;

namespace Core.GitHub.Models
{
    public class Repository
    {
        public int? Id { get; set; }

        [JsonProperty("node_id")]
        public string? NodeId { get; set; }

        public string? Name { get; set; }

        [JsonProperty("full_name")]
        public string? FullName { get; set; }

        public bool? Private { get; set; }

        public User? Owner { get; set; }

        [JsonProperty("html_url")]
        public Uri? HtmlUrl { get; set; }

        public Uri? Description { get; set; }

        public bool? Fork { get; set; }

        public Uri? Url { get; set; }

        [JsonProperty("forks_url")]
        public Uri? ForksUrl { get; set; }

        [JsonProperty("keys_url")]
        public Uri? KeysUrl { get; set; }

        [JsonProperty("collaborators_url")]
        public Uri? CollaboratorsUrl { get; set; }

        [JsonProperty("teams_url")]
        public Uri? TeamsUrl { get; set; }

        [JsonProperty("hooks_url")]
        public Uri? HooksUrl { get; set; }

        [JsonProperty("issue_events_url")]
        public Uri? IssueEventsUrl { get; set; }

        [JsonProperty("events_url")]
        public Uri? EventsUrl { get; set; }

        [JsonProperty("assignees_url")]
        public Uri? AssigneesUrl { get; set; }

        [JsonProperty("branches_url")]
        public Uri? BranchesUrl { get; set; }

        [JsonProperty("tags_url")]
        public Uri? TagsUrl { get; set; }

        [JsonProperty("blobs_url")]
        public Uri? BlobsUrl { get; set; }

        [JsonProperty("git_tags_url")]
        public Uri? GitTagsUrl { get; set; }

        [JsonProperty("git_refs_url")]
        public Uri? GitRefsUrl { get; set; }

        [JsonProperty("trees_url")]
        public Uri? TreesUrl { get; set; }

        [JsonProperty("statuses_url")]
        public Uri? StatusesUrl { get; set; }

        [JsonProperty("languages_url")]
        public Uri? LanguagesUrl { get; set; }

        [JsonProperty("stargazers_url")]
        public Uri? StargazersUrl { get; set; }

        [JsonProperty("contributors_url")]
        public Uri? ContributorsUrl { get; set; }

        [JsonProperty("subscribers_url")]
        public Uri? SubscribersUrl { get; set; }

        [JsonProperty("subscription_url")]
        public Uri? SubscriptionUrl { get; set; }

        [JsonProperty("commits_url")]
        public Uri? CommitsUrl { get; set; }

        [JsonProperty("git_commits_url")]
        public Uri? GitCommitsUrl { get; set; }

        [JsonProperty("comments_url")]
        public Uri? CommentsUrl { get; set; }

        [JsonProperty("issue_comment_url")]
        public Uri? IssueCommentUrl { get; set; }

        [JsonProperty("contents_url")]
        public Uri? ContentsUrl { get; set; }

        [JsonProperty("compare_url")]
        public Uri? CompareUrl { get; set; }

        [JsonProperty("merges_url")]
        public Uri? MergesUrl { get; set; }

        [JsonProperty("archive_url")]
        public Uri? ArchiveUrl { get; set; }

        [JsonProperty("downloads_url")]
        public Uri? DownloadsUrl { get; set; }

        [JsonProperty("issues_url")]
        public Uri? IssuesUrl { get; set; }

        [JsonProperty("pulls_url")]
        public Uri? PullsUrl { get; set; }

        [JsonProperty("milestones_url")]
        public Uri? MilestonesUrl { get; set; }

        [JsonProperty("notifications_url")]
        public Uri? NotificationsUrl { get; set; }

        [JsonProperty("labels_url")]
        public Uri? LabelsUrl { get; set; }

        [JsonProperty("releases_url")]
        public Uri? ReleasesUrl { get; set; }

        [JsonProperty("deployments_url")]
        public Uri? DeploymentsUrl { get; set; }

        [JsonProperty("created_at")]
        public DateTimeOffset CreatedAt { get; set; }

        [JsonProperty("updated_at")]
        public DateTimeOffset UpdatedAt { get; set; }

        [JsonProperty("pushed_at")]
        public DateTimeOffset PushedAt { get; set; }

        [JsonProperty("git_url")]
        public Uri? GitUrl { get; set; }

        [JsonProperty("ssh_url")]
        public Uri? SshUrl { get; set; }

        [JsonProperty("clone_url")]
        public Uri? CloneUrl { get; set; }

        [JsonProperty("svn_url")]
        public Uri? SvnUrl { get; set; }

        public string? Homepage { get; set; }

        public int? Size { get; set; }

        [JsonProperty("stargazers_count")]
        public int? StargazersCount { get; set; }

        [JsonProperty("watchers_count")]
        public int? WatchersCount { get; set; }

        public string? Language { get; set; }

        [JsonProperty("has_issues")]
        public bool? HasIssues { get; set; }

        [JsonProperty("has_projects")]
        public bool? HasProjects { get; set; }

        [JsonProperty("has_downloads")]
        public bool? HasDownloads { get; set; }

        [JsonProperty("has_wiki")]
        public bool? HasWiki { get; set; }

        [JsonProperty("has_pages")]
        public bool? HasPages { get; set; }

        [JsonProperty("forks_count")]
        public int? ForksCount { get; set; }

        [JsonProperty("mirror_url")]
        public Uri? MirrorUrl { get; set; }

        public bool? Archived { get; set; }

        public bool? Disabled { get; set; }

        [JsonProperty("open_issues_count")]
        public int? OpenIssuesCount { get; set; }

        public License? License { get; set; }

        public int? Forks { get; set; }

        [JsonProperty("open_issues")]
        public int? OpenIssues { get; set; }

        public int? Watchers { get; set; }

        [JsonProperty("default_branch")]
        public string? DefaultBranch { get; set; }

        public Permissions? Permissions { get; set; }

        [JsonProperty("temp_clone_token")]
        public string? TempCloneToken { get; set; }

        [JsonProperty("allow_squash_merge")]
        public bool? AllowSquashMerge { get; set; }

        [JsonProperty("allow_merge_commit")]
        public bool? AllowMergeCommit { get; set; }

        [JsonProperty("allow_rebase_merge")]
        public bool? AllowRebaseMerge { get; set; }

        [JsonProperty("delete_branch_on_merge")]
        public bool? DeleteBranchOnMerge { get; set; }

        public Repository? Parent { get; set; }

        public Repository? Source { get; set; }

        [JsonProperty("network_count")]
        public int? NetworkCount { get; set; }

        [JsonProperty("subscribers_count")]
        public int? SubscribersCount { get; set; }
    }
}