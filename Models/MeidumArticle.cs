namespace MarkdownToGist.Models
{
    using System;
    using System.Collections.Generic;
    using Newtonsoft.Json;

    public partial class MediumArticle
    {
        [JsonProperty("data")]
        public MediumData Data { get; set; }
    }

    public partial class MediumData
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("authorId")]
        public string AuthorId { get; set; }

        [JsonProperty("url")]
        public Uri Url { get; set; }

        [JsonProperty("canonicalUrl")]
        public Uri CanonicalUrl { get; set; }

        [JsonProperty("publishStatus")]
        public string PublishStatus { get; set; }

        [JsonProperty("license")]
        public string License { get; set; }

        [JsonProperty("licenseUrl")]
        public Uri LicenseUrl { get; set; }

        [JsonProperty("tags")]
        public List<string> Tags { get; set; }
    }
}
