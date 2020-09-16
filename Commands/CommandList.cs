using System;
using System.Collections.Generic;
using System.Linq;
using Autofac.Features.Metadata;
using ManyConsole;
using MarkdownToGist.Interfaces;

namespace MarkdownToGist.Commands
{
    public class CommandList : ICommandList
    {
        public IList<ConsoleCommand> Commands { get; set; }

        public CommandList(IEnumerable<Meta<IConsoleCommand>> commands)
        {
            Commands = commands.OrderBy(x => x.Metadata["Order"]).Select(x => x.Value as ConsoleCommand).ToList();
        }
    }
}
