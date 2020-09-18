using System;
namespace MarkdownToGist.Models
{
    using System;
    using System.Collections.Generic;

    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public partial class DevToArticle
    {
        [JsonProperty("type_of")]
        public string TypeOf { get; set; }

        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("cover_image")]
        public Uri CoverImage { get; set; }

        [JsonProperty("readable_publish_date")]
        public string ReadablePublishDate { get; set; }

        [JsonProperty("social_image")]
        public Uri SocialImage { get; set; }

        [JsonProperty("tag_list")]
        public string TagList { get; set; }

        [JsonProperty("tags")]
        public List<string> Tags { get; set; }

        [JsonProperty("slug")]
        public string Slug { get; set; }

        [JsonProperty("path")]
        public string Path { get; set; }

        [JsonProperty("url")]
        public Uri Url { get; set; }

        [JsonProperty("canonical_url")]
        public Uri CanonicalUrl { get; set; }

        [JsonProperty("comments_count")]
        public long CommentsCount { get; set; }

        [JsonProperty("public_reactions_count")]
        public long PublicReactionsCount { get; set; }

        [JsonProperty("collection_id")]
        public long CollectionId { get; set; }

        [JsonProperty("created_at")]
        public DateTimeOffset CreatedAt { get; set; }

        [JsonProperty("edited_at")]
        public object EditedAt { get; set; }

        [JsonProperty("crossposted_at")]
        public object CrosspostedAt { get; set; }

        [JsonProperty("published_at")]
        public DateTimeOffset PublishedAt { get; set; }

        [JsonProperty("last_comment_at")]
        public DateTimeOffset LastCommentAt { get; set; }

        [JsonProperty("published_timestamp")]
        public DateTimeOffset PublishedTimestamp { get; set; }

        [JsonProperty("body_html")]
        public string BodyHtml { get; set; }

        [JsonProperty("body_markdown")]
        public string BodyMarkdown { get; set; }

        [JsonProperty("user")]
        public User User { get; set; }

        [JsonProperty("organization")]
        public Organization Organization { get; set; }
    }

    public partial class Organization
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("username")]
        public string Username { get; set; }

        [JsonProperty("slug")]
        public string Slug { get; set; }

        [JsonProperty("profile_image")]
        public Uri ProfileImage { get; set; }

        [JsonProperty("profile_image_90")]
        public Uri ProfileImage90 { get; set; }
    }

    public partial class User
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("username")]
        public string Username { get; set; }

        [JsonProperty("twitter_username")]
        public string TwitterUsername { get; set; }

        [JsonProperty("github_username")]
        public string GithubUsername { get; set; }

        [JsonProperty("website_url")]
        public Uri WebsiteUrl { get; set; }

        [JsonProperty("profile_image")]
        public Uri ProfileImage { get; set; }

        [JsonProperty("profile_image_90")]
        public Uri ProfileImage90 { get; set; }
    }
}
