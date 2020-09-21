using System;
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

        public async Task<DevToArticle> Publish(string content, string apiKey, bool isPublished = false)
        {
            try
            {
                var (title, tags, body) = content.ToTitleTagsBody();
                var request = new DevToArticleRequest
                {
                    Article = new Article
                    {
                        Published = isPublished,
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
    }
}
