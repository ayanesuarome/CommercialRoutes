using Autofac;
using Imperial.CommercialRoutes.WebApi.IoC.Modules;

namespace Imperial.CommercialRoutes.WebApi.IoC
{
    /// <summary>
    /// Registers all the dependencies with Autofac.
    /// </summary>
    public class DependencyRegistrar
    {
        /// <summary>
        /// Registers the services in the provided Autofac <paramref name="builder"/> .
        /// using the provided options.
        /// </summary>
        /// <param name="builder">Autofac container builder to register services on it.</param>
        public void Register(ContainerBuilder builder)
        {
            builder.RegisterModule<LoggingModule>();
            builder.RegisterModule<AutoMapperModule>();
            builder.RegisterModule<DbContextModule>();
            builder.RegisterModule<RestClientModule>();
            builder.RegisterModule<RepositoriesModule>();
            builder.RegisterModule<ApplicationServicesModule>();
            builder.RegisterModule<DomainServicesModule>();
            builder.RegisterModule<WebServicesModule>();
        }
    }
}
