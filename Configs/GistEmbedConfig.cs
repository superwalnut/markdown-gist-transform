using System;
using System.Collections.Generic;

namespace MarkdownToGist.Configs
{
    public class GistEmbedConfig
    {
        public string GistBaseUrl { get; set; }
        public Dictionary<string, string> EmbedTemplate { get; set; }
    }
}
