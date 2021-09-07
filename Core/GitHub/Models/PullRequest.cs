using Newtonsoft.Json;

using System;
using System.Collections.Generic;

namespace Core.GitHub.Models
{
    public class PullRequest
    {
        public Uri? Url { get; set; }

        public long? Id { get; set; }

        [JsonProperty("node_id")]
        public string? NodeId { get; set; }

        [JsonProperty("html_url")]
        public Uri? HtmlUrl { get; set; }

        [JsonProperty("diff_url")]
        public Uri? DiffUrl { get; set; }

        [JsonProperty("patch_url")]
        public Uri? PatchUrl { get; set; }

        [JsonProperty("issue_url")]
        public Uri? IssueUrl { get; set; }

        public long? Number { get; set; }

        public string? State { get; set; }

        public bool? Locked { get; set; }

        public string? Title { get; set; }

        public User? User { get; set; }

        public string? Body { get; set; }

        [JsonProperty("created_at")]
        public DateTimeOffset? CreatedAt { get; set; }

        [JsonProperty("updated_at")]
        public DateTimeOffset? UpdatedAt { get; set; }

        [JsonProperty("closed_at")]
        public DateTimeOffset? ClosedAt { get; set; }

        [JsonProperty("merged_at")]
        public DateTimeOffset? MergedAt { get; set; }

        [JsonProperty("merge_commit_sha")]
        public string? MergeCommitSha { get; set; }

        public object? Assignee { get; set; }

        public IList<object>? Assignees { get; set; }

        [JsonProperty("requested_reviewers")]
        public IList<object>? RequestedReviewers { get; set; }

        [JsonProperty("requested_teams")]
        public IList<Team>? RequestedTeams { get; set; }

        public IList<object>? Labels { get; set; }

        public object? Milestone { get; set; }

        public bool? Draft { get; set; }

        [JsonProperty("commits_url")]
        public Uri? CommitsUrl { get; set; }

        [JsonProperty("review_comments_url")]
        public Uri? ReviewCommentsUrl { get; set; }

        [JsonProperty("review_comment_url")]
        public string? ReviewCommentUrl { get; set; }

        [JsonProperty("comments_url")]
        public Uri? CommentsUrl { get; set; }

        [JsonProperty("statuses_url")]
        public Uri? StatusesUrl { get; set; }

        public PullRequestContext? Head { get; set; }

        public PullRequestContext? Base { get; set; }

        [JsonProperty("_links")]
        public RichLinks? Links { get; set; }

        [JsonProperty("author_association")]
        public string? AuthorAssociation { get; set; }

        [JsonProperty("auto_merge")]
        public object? AutoMerge { get; set; }

        [JsonProperty("active_lock_reason")]
        public object? ActiveLockReason { get; set; }
    }
}