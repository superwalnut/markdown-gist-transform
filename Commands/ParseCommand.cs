using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MarkdownToGist.Attributes;
using MarkdownToGist.Configs;
using MarkdownToGist.Interfaces;
using MarkdownToGist.Services;
using Microsoft.Extensions.Options;
using Serilog;

namespace MarkdownToGist.Commands
{
    [AutofacRegistrationOrder(1)]
    public class ParseCommand : BaseConsoleCommand
    {
        private readonly ILogger _logger;
        private readonly IMarkdownService _markdownService;
        private readonly IOptions<GistEmbedConfig> _gistConfig;
        private string _filePath;
        private string _token;

        public ParseCommand(IMarkdownService markdownService, IOptions<GistEmbedConfig> gistConfig, ILogger logger)
        {
            _markdownService = markdownService;
            _gistConfig = gistConfig;
            _logger = logger;

            this.IsCommand("parse", "Parse markdown codes with embeded gist code");
            this.HasOption("f|file=", "markdown file path", v => _filePath = v);
            this.HasOption("t|token=", "github auth token", v => _token = v);
        }

        public override int RunCommand()
        {
            if (string.IsNullOrEmpty(_token))
            {
                PrintErrorLine("You must provide a github auth token");
                return -1;
            }                

            if (string.IsNullOrEmpty(_filePath))
                _filePath = Environment.CurrentDirectory;

            var files = FindMdFiles(_filePath);

            if(files.Count<=0)
            {
                PrintErrorLine("No .md files are found");
            }

            foreach(var file in files)
            {
                FileInfo fi = new FileInfo(file);
                var fileName = Path.GetFileNameWithoutExtension(fi.Name);

                if (HasProcessed(fi))
                {
                    _logger.Information("{file} is already parsed", fi.Name);
                    continue;
                }
                    
                var md = ReadMdFile(file);
                var sources = _markdownService.ParseEmbedGist(fileName, md, _token).Result;

                if(sources != null)
                {
                    foreach (var s in sources)
                    {
                        var newPath = Path.Combine(fi.Directory.FullName, $"{fileName}-[{s.Key}].md");
                        SaveMdFile(newPath, s.Value);
                    }
                }
            }

            return 0;
        }

        private List<string> FindMdFiles(string path)
        {
            var list = new List<string>();
            // if it a md file, return the path
            if (File.Exists(path) && Path.GetExtension(path).Equals(".md"))
            {
                return new List<string> { path };
            }

            // if it is a folder, we need to discover all md files inside
            if (Directory.Exists(path))
            {
                var mdFiles = Directory.GetFiles(path, "*.md", SearchOption.AllDirectories);
                return mdFiles.ToList();
            }

            throw new FileNotFoundException(".md files are not found");
        }

        private string ReadMdFile(string path)
        {
            using(var reader = new StreamReader(path))
            {
                return reader.ReadToEnd();
            }
        }

        private void SaveMdFile(string path, string parsedContent)
        {
            using(var writer = new StreamWriter(path))
            {
                writer.Write(parsedContent);
                writer.Flush();
            }
        }

        private bool HasProcessed(FileInfo fi)
        {
            var fileName = Path.GetFileNameWithoutExtension(fi.Name);
            var keys = _gistConfig.Value.EmbedTemplate.Keys;
            foreach(var key in keys)
            {
                var keyPath = Path.Combine(fi.Directory.FullName, $"{fileName}-[{key}].md");
                if(!File.Exists(keyPath))
                {
                    return false;
                }
            }

            return true;
        }

    }
}
