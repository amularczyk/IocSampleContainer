using System.Web.Http;
using Autofac;
using Autofac.Features.AttributeFilters;
using Autofac.Integration.WebApi;
using UsageCore;
using UsageFramework.Controllers;

namespace UsageFramework
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                "DefaultApi",
                "api/{controller}/{id}",
                new {id = RouteParameter.Optional}
            );

            AddAutofac(config);
        }

        private static void AddAutofac(HttpConfiguration config)
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<SingletonClass>().As<ISingletonClass>().SingleInstance();
            builder.RegisterType(typeof(TransientClass)).As(typeof(ITransientClass));
            builder
                .Register<IScopedClass>(provider => new ScopedClass(provider.Resolve<ITransientClass>()))
                .InstancePerLifetimeScope();

            builder.RegisterType<TransientClass2>().Keyed<ITransientClass>("name2");
            builder.RegisterType<SampleClass>().As<ISampleClass>().WithAttributeFiltering();

            // Important: Need to register also controllers
            builder.RegisterType<ValuesController>();

            var container = builder.Build();

            var resolver = new AutofacWebApiDependencyResolver(container);
            config.DependencyResolver = resolver;
        }
    }
}