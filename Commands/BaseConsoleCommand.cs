using System;
using System.Diagnostics;
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

    }
}
