using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using ManyConsole;
using MarkdownToGist.Interfaces;

namespace MarkdownToGist.Commands
{
    public abstract class BaseConsoleCommand : ConsoleCommand, IConsoleCommand
    {
        public override int Run(string[] remainingArguments)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            int errorCode = 0;
            try
            {
                errorCode = RunCommand();
            }
            catch (Exception ex)
            {
                errorCode = 999;
                PrintErrorLine(ex.Message);
            }
            finally
            {
                PrintSuccessLine($"Running for {sw.Elapsed.TotalSeconds} seconds");
                PrintJobCompleted(errorCode);
            }

            return errorCode;
        }

        public virtual int RunCommand()
        {
            throw new NotImplementedException("command is not implemented");
        }


        protected void PrintTitleLine(string title)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(title);
            Console.ResetColor();
        }

        protected void PrintSubTitleLine(string sub)
        {
            Console.WriteLine(sub);
            Console.WriteLine("---------------------------------------------");
        }

        protected void PrintSplitLine()
        {
            Console.WriteLine();
            Console.WriteLine("              **   **   **   **              ");
            Console.WriteLine();
        }

        protected void PrintSuccessLine(string line)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(line);
            Console.ResetColor();
        }

        protected void PrintErrorLine(string line)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(line);
            Console.ResetColor();
        }

        protected void PrintJobCompleted(int code)
        {
            Console.ForegroundColor = code != 0 ? ConsoleColor.Red : ConsoleColor.Green;
            Console.WriteLine($"Job completed with code {code} ...");
            Console.ResetColor();
        }

        protected List<string> FindMdFiles(string path, string pattern = "*.md")
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
                var mdFiles = Directory.GetFiles(path, pattern, SearchOption.AllDirectories);
                return mdFiles.ToList();
            }

            throw new FileNotFoundException(".md files are not found");
        }

        protected string ReadFile(string path)
        {
            using (var reader = new StreamReader(path))
            {
                return reader.ReadToEnd();
            }
        }

        protected string GetPublishedFileName(string path, string brand)
        {
            FileInfo fi = new FileInfo(path);
            var fileName = Path.GetFileNameWithoutExtension(fi.Name);
            var publishedFile = Path.Combine(fi.Directory.FullName, $"{fileName}-[{brand}].published.md");
            return publishedFile;
        }

        protected bool FindPublishedFile(string path, string brand)
        {            
            return File.Exists(GetPublishedFileName(path, brand));
        }

        protected void SaveMdFile(string path, string parsedContent)
        {
            using (var writer = new StreamWriter(path))
            {
                writer.Write(parsedContent);
                writer.Flush();
            }
        }
    }
}
