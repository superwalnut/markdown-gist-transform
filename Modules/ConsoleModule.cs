using System;
namespace MarkdownToGist.Modules
{
    using Autofac;
    using AutofacSerilogIntegration;
    using MarkdownToGist.AutoMapper;
    using global::AutoMapper.Contrib.Autofac.DependencyInjection;
    using System.Linq;
    using MarkdownToGist.Commands;
    using MarkdownToGist.Interfaces;
    using MarkdownToGist.Attributes;
    using MarkdownToGist.Services;

    public class ConsoleModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            // To register dependencies
            builder.AddAutoMapper(x=>x.AddProfile<MyAutoMapperProfile>());

            builder.RegisterType<GistService>().As<IGistService>();
            builder.RegisterType<MarkdownService>().As<IMarkdownService>();

            var types = typeof(BaseConsoleCommand).Assembly.GetTypes().Where(x => x.IsAssignableTo<IConsoleCommand>() & !x.IsInterface & !x.IsAbstract);

            foreach (var t in types)
            {
                var orderAttribute = t.GetCustomAttributes(typeof(AutofacRegistrationOrderAttribute), false).OfType<AutofacRegistrationOrderAttribute>().FirstOrDefault();
                builder.RegisterType(t).AsImplementedInterfaces().WithMetadata(AutofacRegistrationOrderAttribute.AttributeName, orderAttribute?.Order);
            }

            builder.RegisterType<CommandList>().As<ICommandList>();

        }
    }
}
