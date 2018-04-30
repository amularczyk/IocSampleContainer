using System.Web.Http;
using Autofac;
using Autofac.Integration.WebApi;
using UsageCore;

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

            builder.RegisterType<TransientClass2>().Named<ITransientClass>("name");
            builder.RegisterType<SampleClass>().As<ISampleClass>();

            var container = builder.Build();

            var resolver = new AutofacWebApiDependencyResolver(container);
            config.DependencyResolver = resolver;
        }
    }
}