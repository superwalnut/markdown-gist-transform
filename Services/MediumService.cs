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
    public class MediumService : IMediumService
    {
        private readonly IOptions<MediumConfig> _mediumConfig;
        private readonly ILogger _logger;

        public MediumService(IOptions<MediumConfig> mediumConfig, ILogger logger)
        {
            _mediumConfig = mediumConfig;
            _logger = logger;
        }

        public async Task<MediumArticle> Publish(string content, string authToken, bool isPublished = false)
        {
            try
            {
                var (title, tags, body) = content.ToTitleTagsBody();
                var request = new MediumArticleRequest
                {
                    Title = title,
                    Tags = tags.ToList(),
                    PublishStatus = isPublished? "public" : "draft",
                    ContentFormat = "markdown",
                    Content = body
                };

                var user = await GetUser(authToken);

                var postUrl = string.Format(_mediumConfig.Value.ArticleApi, user.Data.Id);

                var article = await postUrl
                    .WithOAuthBearerToken(authToken)
                    .WithHeader("Content-Type", "application/json")
                    .WithHeader("User-Agent", "request")
                    .PostJsonAsync(request)
                    .ReceiveJson<MediumArticle>();

                _logger.Information("publishing a article to Medium {name} {link}", request.Title, article.Data.Url);

                return article;
            }
            catch (Exception ex)
            {
                _logger.Error("publishing Medium error", ex);
            }

            return null;
        }

        private async Task<MediumUser> GetUser(string authToken)
        {
            var user = await _mediumConfig.Value.ProfileApi
                    .WithOAuthBearerToken(authToken)
                    .WithHeader("Content-Type", "application/json")
                    .WithHeader("User-Agent", "request")
                    .GetAsync()
                    .ReceiveJson<MediumUser>();

            return user;
        }


    }
}
