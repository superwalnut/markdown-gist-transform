using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MarkdownToGist.Configs;
using MarkdownToGist.Extensions;
using MarkdownToGist.Interfaces;
using MarkdownToGist.Models;
using Microsoft.Extensions.Options;

namespace MarkdownToGist.Services
{
    public class MarkdownService : IMarkdownService
    {
        private static Regex _codeRegex = new Regex(@"(```[a-z\W]*\n[\s\S]*?\n```)");
        private readonly IGistService _gistService;
        private readonly IOptions<GistEmbedConfig> _gistConfig;

        public MarkdownService(IGistService gistService, IOptions<GistEmbedConfig> gistConfig)
        {
            _gistService = gistService;
            _gistConfig = gistConfig;
        }

        public async Task<Dictionary<string,string>> ParseEmbedGist(string fileName, string markdown, string authToken)
        {
            var result = markdown;

            var sources = _gistConfig.Value.EmbedTemplate.Keys.ToList().ToDictionary(x=>x, y=> markdown);
            var keys = _gistConfig.Value.EmbedTemplate.Keys;

            if (!string.IsNullOrEmpty(markdown) && _codeRegex.IsMatch(markdown))
            {
                var matches = _codeRegex.Matches(markdown);

                if (matches.Count == 0)
                    return null;

                int index = 0;
                foreach (Match match in matches)
                {
                    var gist = await _gistService.Create($"{fileName}-{index}", match.Value, authToken);
                    if (gist != null)
                    {
                        foreach(var key in keys)
                        {
                            sources[key] = sources[key].Replace(match.Value, GetEmbedContent(gist, key));
                        }
                    }

                    index++;
                }

                return sources;
            }

            return null;
        }

        private string GetEmbedContent(Gist gist, string source)
        {
            var template = _gistConfig.Value.EmbedTemplate[source];

            return string.Format(template, gist.Owner.Login, gist.Id);
        }

    }
}
