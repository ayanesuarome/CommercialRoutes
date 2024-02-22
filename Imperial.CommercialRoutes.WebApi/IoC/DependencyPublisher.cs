using Autofac.Integration.WebApi;
using Autofac;
using System.Reflection;
using System.Web.Http;
namespace Imperial.CommercialRoutes.WebApi.IoC
{
    /// <summary>
    /// Register all services of the entire application.
    /// </summary>
    public class DependencyPublisher
    {
        /// <summary>
        /// Register all services of the entire application.
        /// This method must be called as soon as the app starts so that the app can use DI.
        /// </summary>
        /// <param name="configuration">Http server configuration.</param>
        public virtual void RegisterDependencies(HttpConfiguration configuration)
        {
            ContainerBuilder builder = new ContainerBuilder();
            new DependencyRegistrar()
                .Register(builder);

            // Registering Web.API Controllers
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            // Register the Autofac filter provider
            builder.RegisterWebApiFilterProvider(configuration);
            // Register the Autofac model binder provider
            builder.RegisterWebApiModelBinderProvider();

            // Create and assign a dependency resolver for Web API to use.
            IContainer container = builder.Build();
            configuration.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }
    }
}
