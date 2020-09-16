using System;
namespace MarkdownToGist.Models
{
    using System;
    using System.Collections.Generic;

    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public partial class GistRequest
    {
        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("public")]
        public bool Public { get; set; }

        [JsonProperty("files")]
        public Dictionary<string, File> Files { get; set; }
    }

    public partial class File
    {
        [JsonProperty("content")]
        public string Content { get; set; }
    }
}
