using System;
using System.Collections.Generic;
using ManyConsole;

namespace MarkdownToGist.Interfaces
{
    public interface ICommandList
    {
        IList<ConsoleCommand> Commands { get; set; }
    }
}
