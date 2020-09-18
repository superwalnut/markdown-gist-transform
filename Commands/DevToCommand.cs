using System;
using MarkdownToGist.Attributes;
using MarkdownToGist.Interfaces;
using Serilog;

namespace MarkdownToGist.Commands
{
    [AutofacRegistrationOrder(2)]
    public class DevToCommand : BaseConsoleCommand
    {
        private readonly ILogger _logger;
        private readonly IDevToService _devToService;
        private string _filePath;
        private string _apikey;

        public DevToCommand(ILogger logger, IDevToService devToService)
        {
            _logger = logger;
            _devToService = devToService;

            this.IsCommand("devto", "publish artile to dev.to");
            this.HasOption("f|file=", "markdown file path", v => _filePath = v);
            this.HasOption("k|apiKey=", "dev.to api key", v => _apikey = v);
        }

        public override int RunCommand()
        {
            if (string.IsNullOrEmpty(_apikey))
            {
                PrintErrorLine("You must provide a dev.to api key");
                return -1;
            }

            if (string.IsNullOrEmpty(_filePath))
                _filePath = Environment.CurrentDirectory;

            var files = FindMdFiles(_filePath, "*-[Dev.to].md");

            if (files.Count <= 0)
            {
                PrintErrorLine("No parsed dev.to md files are found");
            }

            foreach(var file in files)
            {
                var content = ReadMdFile(file);

                var article = _devToService.Publish(content, _apikey).Result;

                PrintSuccessLine($"published to dev.to {article.Url}");
            }

            return 0;
        }
    }
}
