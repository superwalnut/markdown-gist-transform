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
        private static Regex _metaRegex = new Regex(@"(---[a-z\W]*\n[\s\S]*?\n---)");

        public MediumService(IOptions<MediumConfig> mediumConfig, ILogger logger)
        {
            _mediumConfig = mediumConfig;
            _logger = logger;
        }

        public async Task<MediumArticle> Publish(string content, string authToken, bool isPublished = false)
        {
            try
            {
                var (title, tags, body) = GetTitleTagsBody(content);
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

        private (string, string[], string) GetTitleTagsBody(string content)
        {
            if (_metaRegex.IsMatch(content))
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
                        title = titleRegex.Match(line).Replace(line, "").Replace("\"", "");
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
