using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Autofac;
using AutoMapper.Contrib.Autofac.DependencyInjection;
using ManyConsole;
using MarkdownToGist.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace MarkdownToGist
{
    class Program
    {        
        static int Main(string[] args)
        {
            var startup = new Startup();
            var serviceProvider = startup.BuildContainer();

            var list = serviceProvider.GetService<ICommandList>();
            return ConsoleCommandDispatcher.DispatchCommand(list.Commands, args, Console.Out);
        }
    }
}
