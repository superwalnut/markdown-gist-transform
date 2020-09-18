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
    using Flurl.Http;
    using Newtonsoft.Json;
    using Flurl.Http.Configuration;

    public class ConsoleModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            // To register dependencies
            builder.AddAutoMapper(x=>x.AddProfile<MyAutoMapperProfile>());

            builder.RegisterType<GistService>().As<IGistService>();
            builder.RegisterType<MarkdownService>().As<IMarkdownService>();
            builder.RegisterType<DevToService>().As<IDevToService>();
            builder.RegisterType<MediumService>().As<IMediumService>();

            var types = typeof(BaseConsoleCommand).Assembly.GetTypes().Where(x => x.IsAssignableTo<IConsoleCommand>() & !x.IsInterface & !x.IsAbstract);

            foreach (var t in types)
            {
                var orderAttribute = t.GetCustomAttributes(typeof(AutofacRegistrationOrderAttribute), false).OfType<AutofacRegistrationOrderAttribute>().FirstOrDefault();
                builder.RegisterType(t).AsImplementedInterfaces().WithMetadata(AutofacRegistrationOrderAttribute.AttributeName, orderAttribute?.Order);
            }

            builder.RegisterType<CommandList>().As<ICommandList>();

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
    }
}
