using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Flurl.Http;
using MarkdownToGist.Configs;
using MarkdownToGist.Extensions;
using MarkdownToGist.Interfaces;
using MarkdownToGist.Models;
using Microsoft.Extensions.Options;
using Serilog;

namespace MarkdownToGist.Services
{
    public class DevToService : IDevToService
    {
        private readonly IOptions<DevToConfig> _devtoConfig;
        private readonly ILogger _logger;
        private static Regex _metaRegex = new Regex(@"(---[a-z\W]*\n[\s\S]*?\n---)");

        public DevToService(IOptions<DevToConfig> devtoConfig, ILogger logger)
        {
            _devtoConfig = devtoConfig;
            _logger = logger;
        }

        public async Task<DevToArticle> Publish(string content, string apiKey)
        {
            try
            {
                var (title, tags, body) = GetTitleTagsBody(content);
                var request = new DevToArticleRequest
                {
                    Article = new Article
                    {
                        Published = true,
                        Title = title,
                        Tags = tags.ToList(),
                        BodyMarkdown = body
                    }
                };

                var article = await _devtoConfig.Value.ArticleApi
                    .WithHeader("api_key", apiKey)
                    .WithHeader("Content-Type", "application/json")
                    .WithHeader("User-Agent", "request")
                    .PostJsonAsync(request)
                    .ReceiveJson<DevToArticle>();

                _logger.Information("publishing a article to dev.to {name} {link}", request.Article.Title, article.Url);

                return article;
            }
            catch (Exception ex)
            {
                _logger.Error("publishing dev.to error", ex);
            }

            return null;
        }

        private (string,string[],string) GetTitleTagsBody(string content)
        {
            if(_metaRegex.IsMatch(content))
            {
                var meta = _metaRegex.Match(content);

                string line = null, title = "", tagVal = "", body = "";
                var titleRegex = new Regex(@"\s*title\s*:");
                var tagsRegex = new Regex(@"\s*tags\s*:");

                body = content.Replace(meta.Value, "");

                StringReader strReader = new StringReader(meta.Value);
                while (true)
                {
                    line = strReader.ReadLine();
                    if (line == null)
                        break;

                    if (titleRegex.IsMatch(line))
                    {
                        title = titleRegex.Match(line).Replace(line, "").Replace("\"","");
                    }

                    if (tagsRegex.IsMatch(line))
                    {
                        tagVal = tagsRegex.Match(line).Replace(line, "");
                    }
                }

                var tags = tagVal.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                return (title, tags, body);
            }

            return (null, null, null);
        }
    }
}
