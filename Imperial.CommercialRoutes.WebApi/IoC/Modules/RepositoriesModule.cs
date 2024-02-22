using Autofac;
using Imperial.CommercialRoutes.Domain.Interfaces;
using Imperial.CommercialRoutes.Infrastructure.Repositories;

namespace Imperial.CommercialRoutes.WebApi.IoC.Modules
{
    /// <summary>
    /// Autofac module to add repositories.
    /// </summary>
    public class RepositoriesModule : Module
    {
        /// <summary>
        /// Adds registrations to the container.
        /// </summary>
        /// <param name="builder">The builder through which components can be registered.</param>
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<PlanetRepository>()
                .As<IPlanetRepository>()
                .InstancePerLifetimeScope();
            
            builder.RegisterType<DistanceRepository>()
                .As<IDistanceRepository>()
                .InstancePerLifetimeScope();
        }
    }
}
