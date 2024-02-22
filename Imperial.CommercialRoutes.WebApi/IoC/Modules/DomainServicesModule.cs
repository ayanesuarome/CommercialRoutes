using Autofac;
using Imperial.CommercialRoutes.Domain.Interfaces;
using Imperial.CommercialRoutes.Domain.Services;

namespace Imperial.CommercialRoutes.WebApi.IoC.Modules
{
    /// <summary>
    /// Autofac module to add domain services.
    /// </summary>
    public class DomainServicesModule : Module
    {
        /// <summary>
        /// Adds registrations to the container.
        /// </summary>
        /// <param name="builder">The builder through which components can be registered.</param>
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<SecurityPriceService>()
                .As<ISecurityPriceService>()
                .InstancePerLifetimeScope();
            
            builder.RegisterType<RouteService>()
                .As<IRouteService>()
                .InstancePerLifetimeScope();
        }
    }
}
