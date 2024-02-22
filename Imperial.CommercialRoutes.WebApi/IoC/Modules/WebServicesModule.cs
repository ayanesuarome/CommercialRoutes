using Autofac;
using Imperial.CommercialRoutes.Application.Interfaces.WebServices;
using Imperial.CommercialRoutes.Infrastructure.WebServices;

namespace Imperial.CommercialRoutes.WebApi.IoC.Modules
{
    /// <summary>
    /// Autofac module to add web services.
    /// </summary>
    public class WebServicesModule : Module
    {
        /// <summary>
        /// Adds registrations to the container.
        /// </summary>
        /// <param name="builder">The builder through which components can be registered.</param>
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<SindicatePlanetWebService>()
                .As<ISindicatePlanetWebService>()
                .InstancePerLifetimeScope();

            builder.RegisterType<SindicateDistanceWebService>()
                .As<ISindicateDistanceWebService>()
                .InstancePerLifetimeScope();
            
            builder.RegisterType<EmpireRebelInfluenceWebService>()
                .As<IEmpireRebelInfluenceWebService>()
                .InstancePerLifetimeScope();
            
            builder.RegisterType<EmpirePriceWebService>()
                .As<IEmpirePriceWebService>()
                .InstancePerLifetimeScope();

            builder.RegisterType<EmpireAircraftWebService>()
                .As<IEmpireAircraftWebService>()
                .InstancePerLifetimeScope();
        }
    }
}
