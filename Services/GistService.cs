using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Flurl.Http;
using Flurl.Http.Configuration;
using MarkdownToGist.Configs;
using MarkdownToGist.Extensions;
using MarkdownToGist.Interfaces;
using MarkdownToGist.Models;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Serilog;

namespace MarkdownToGist.Services
{
    public class GistService : IGistService
    {
        private readonly IOptions<GithubConfig> _githubConfig;
        private readonly ILogger _logger;
        private static readonly Regex _regex = new Regex(@"(```[a-z\W]*)");

        public GistService(IOptions<GithubConfig> githubConfig, ILogger logger)
        {
            _githubConfig = githubConfig;
            _logger = logger;

            FlurlHttp.Configure(settings => {
                var jsonSettings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    ObjectCreationHandling = ObjectCreationHandling.Replace,
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                };
                settings.JsonSerializer = new NewtonsoftJsonSerializer(jsonSettings);
            });
        }

        public async Task<Gist> Create(string file, string content, string authToken)
        {
            try
            {
                var request = new GistRequest
                {
                    Description = $"code block for {file}",
                    Public = true,
                    Files = new Dictionary<string, File> { { file, new File { Content = RemoveBackticks(content) } } }
                };

                var gist = await _githubConfig.Value.GistApi
                    .WithOAuthBearerToken(authToken)
                    .WithHeader("Accept", "application/vnd.github.v3+json")
                    .WithHeader("Content-Type", "application/json")
                    .WithHeader("User-Agent", "request")
                    .PostJsonAsync(request)
                    .ReceiveJson<Gist>();

                _logger.Information("creating a gist {name} {link}", file, gist.HtmlUrl);

                return gist;
            }
            catch(Exception ex)
            {
                _logger.Error("Creating gist error",ex);
            }

            return null;
        }

        private string RemoveBackticks(string content)
        {
            var result = content;
            var matches = _regex.Matches(content);
            foreach(Match match in matches)
            {
                result = result.Replace(match.Value, "");
            }
            return result;
        }
    }
}
