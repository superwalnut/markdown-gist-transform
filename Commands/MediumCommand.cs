using System;
using MarkdownToGist.Attributes;
using MarkdownToGist.Interfaces;
using MarkdownToGist.Models;
using Serilog;

namespace MarkdownToGist.Commands
{
    [AutofacRegistrationOrder(3)]
    public class MediumCommand : BaseConsoleCommand
    {
        private readonly ILogger _logger;
        private readonly IMediumService _mediumService;
        private string _filePath;
        private string _token;
        private string _publishStatus;

        public MediumCommand(ILogger logger, IMediumService mediumService)
        {
            _logger = logger;
            _mediumService = mediumService;

            this.IsCommand("medium", "publish artile to medium");
            this.HasOption("f|file=", "markdown file path", v => _filePath = v);
            this.HasOption("t|token=", "medium auth token", v => _token = v);
            this.HasOption("s|status=", "publish status public/draft", v => _publishStatus = v);
        }

        public override int RunCommand()
        {
            if (string.IsNullOrEmpty(_token))
            {
                PrintErrorLine("You must provide a medium outh token");
                return -1;
            }

            if (string.IsNullOrEmpty(_filePath))
                _filePath = Environment.CurrentDirectory;

            var files = FindMdFiles(_filePath, $"*-[{Brands.Medium}].md");

            if (files.Count <= 0)
            {
                PrintErrorLine("No parsed medium md files are found");
            }

            var isPublish = !string.IsNullOrEmpty(_publishStatus) && _publishStatus.Equals("publish", StringComparison.CurrentCultureIgnoreCase);

            foreach (var file in files)
            {
                var published = FindPublishedFile(file, Brands.Medium);

                if (published)
                {
                    PrintErrorLine($"File {file} is already published to medium");
                    continue;
                }

                var content = ReadFile(file);

                var article = _mediumService.Publish(content, _token, isPublish).Result;

                // save published file
                SaveMdFile(GetPublishedFileName(file, Brands.Medium), content);

                PrintSuccessLine($"published to medium {article.Data.Url}");
            }

            return 0;
        }
    }
}
