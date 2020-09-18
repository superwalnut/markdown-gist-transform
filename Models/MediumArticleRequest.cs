using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace MarkdownToGist.Models
{
    public partial class MediumArticleRequest
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("contentFormat")]
        public string ContentFormat { get; set; }

        [JsonProperty("content")]
        public string Content { get; set; }

        [JsonProperty("canonicalUrl")]
        public Uri CanonicalUrl { get; set; }

        [JsonProperty("tags")]
        public List<string> Tags { get; set; }

        [JsonProperty("publishStatus")]
        public string PublishStatus { get; set; }
    }
}
