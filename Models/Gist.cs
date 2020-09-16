using System;
namespace MarkdownToGist.Models
{
    using System;
    using System.Collections.Generic;

    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public partial class Gist
    {
        [JsonProperty("url")]
        public Uri Url { get; set; }

        [JsonProperty("forks_url")]
        public Uri ForksUrl { get; set; }

        [JsonProperty("commits_url")]
        public Uri CommitsUrl { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("node_id")]
        public string NodeId { get; set; }

        [JsonProperty("git_pull_url")]
        public Uri GitPullUrl { get; set; }

        [JsonProperty("git_push_url")]
        public Uri GitPushUrl { get; set; }

        [JsonProperty("html_url")]
        public Uri HtmlUrl { get; set; }

        [JsonProperty("files")]
        public Object Files { get; set; }

        [JsonProperty("public")]
        public bool Public { get; set; }

        [JsonProperty("created_at")]
        public DateTimeOffset CreatedAt { get; set; }

        [JsonProperty("updated_at")]
        public DateTimeOffset UpdatedAt { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("comments")]
        public long Comments { get; set; }

        [JsonProperty("user")]
        public object User { get; set; }

        [JsonProperty("comments_url")]
        public Uri CommentsUrl { get; set; }

        [JsonProperty("owner")]
        public Owner Owner { get; set; }

        [JsonProperty("forks")]
        public List<object> Forks { get; set; }

        [JsonProperty("history")]
        public List<History> History { get; set; }

        [JsonProperty("truncated")]
        public bool Truncated { get; set; }
    }

    public partial class History
    {
        [JsonProperty("user")]
        public Owner User { get; set; }

        [JsonProperty("version")]
        public string Version { get; set; }

        [JsonProperty("committed_at")]
        public DateTimeOffset CommittedAt { get; set; }

        [JsonProperty("change_status")]
        public ChangeStatus ChangeStatus { get; set; }

        [JsonProperty("url")]
        public Uri Url { get; set; }
    }

    public partial class ChangeStatus
    {
        [JsonProperty("total")]
        public long Total { get; set; }

        [JsonProperty("additions")]
        public long Additions { get; set; }

        [JsonProperty("deletions")]
        public long Deletions { get; set; }
    }

    public partial class Owner
    {
        [JsonProperty("login")]
        public string Login { get; set; }

        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("node_id")]
        public string NodeId { get; set; }

        [JsonProperty("avatar_url")]
        public Uri AvatarUrl { get; set; }

        [JsonProperty("gravatar_id")]
        public string GravatarId { get; set; }

        [JsonProperty("url")]
        public Uri Url { get; set; }

        [JsonProperty("html_url")]
        public Uri HtmlUrl { get; set; }

        [JsonProperty("followers_url")]
        public Uri FollowersUrl { get; set; }

        [JsonProperty("following_url")]
        public string FollowingUrl { get; set; }

        [JsonProperty("gists_url")]
        public string GistsUrl { get; set; }

        [JsonProperty("starred_url")]
        public string StarredUrl { get; set; }

        [JsonProperty("subscriptions_url")]
        public Uri SubscriptionsUrl { get; set; }

        [JsonProperty("organizations_url")]
        public Uri OrganizationsUrl { get; set; }

        [JsonProperty("repos_url")]
        public Uri ReposUrl { get; set; }

        [JsonProperty("events_url")]
        public string EventsUrl { get; set; }

        [JsonProperty("received_events_url")]
        public Uri ReceivedEventsUrl { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("site_admin")]
        public bool SiteAdmin { get; set; }
    }

}
