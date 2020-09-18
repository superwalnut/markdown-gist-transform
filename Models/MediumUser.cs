namespace MarkdownToGist.Models
{
    using System;
    using Newtonsoft.Json;

    public partial class MediumUser
    {
        [JsonProperty("data")]
        public MediumUserData Data { get; set; }
    }

    public partial class MediumUserData
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("username")]
        public string Username { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("url")]
        public Uri Url { get; set; }

        [JsonProperty("imageUrl")]
        public Uri ImageUrl { get; set; }
    }
}
