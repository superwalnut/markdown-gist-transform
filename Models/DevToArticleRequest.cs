using System;
namespace MarkdownToGist.Models
{
    using System;
    using System.Collections.Generic;

    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public partial class DevToArticleRequest
    {
        [JsonProperty("article")]
        public Article Article { get; set; }
    }

    public partial class Article
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("published")]
        public bool Published { get; set; }

        [JsonProperty("body_markdown")]
        public string BodyMarkdown { get; set; }

        [JsonProperty("tags")]
        public List<string> Tags { get; set; }
    }
}
