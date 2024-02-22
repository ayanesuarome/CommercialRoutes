using Autofac;
using Imperial.CommercialRoutes.Application;
using Imperial.CommercialRoutes.Application.Interfaces.Services;
using Imperial.CommercialRoutes.Application.Services;

namespace Imperial.CommercialRoutes.WebApi.IoC.Modules
{
    /// <summary>
    /// Autofac module to add application services.
    /// </summary>
    public class ApplicationServicesModule : Module
    {
        /// <summary>
        /// Adds registrations to the container.
        /// </summary>
        /// <param name="builder">The builder through which components can be registered.</param>
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<PlanetFinderService>()
                .As<IPlanetFinderService>()
                .InstancePerLifetimeScope();
            
            builder.RegisterType<DistanceFinderService>()
                .As<IDistanceFinderService>()
                .InstancePerLifetimeScope();

            builder.RegisterType<AircraftFinderService>()
                .As<IAircraftFinderService>()
                .InstancePerLifetimeScope();

            builder.RegisterType<BreakDownRoutePriceService>()
                .As<IBreakDownRoutePriceService>()
                .InstancePerLifetimeScope();

            builder.RegisterType<CommercialRoutesService>()
                .As<ICommercialRoutesService>()
                .InstancePerLifetimeScope();
        }
    }
}
