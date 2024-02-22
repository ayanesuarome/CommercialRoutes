using Autofac;
using AutoMapper;
using Imperial.CommercialRoutes.Application.AutoMapper;

namespace Imperial.CommercialRoutes.WebApi.IoC.Modules
{
    /// <summary>
    /// Autofac module to add mapper registrations.
    /// </summary>
    public class AutoMapperModule : Module
    {
        #region Methods

        /// <summary>
        /// Adds registrations to the container.
        /// </summary>
        /// <param name="builder">The builder through which components can be registered.</param>
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(_ => AutoMapperConfiguration.RegisterMappings())
                .AsSelf()
                .As<IConfigurationProvider>()
                .SingleInstance();

            builder.Register(ctx => ctx.Resolve<IConfigurationProvider>().CreateMapper(ctx.Resolve))
                .As<IMapper>()
                .InstancePerLifetimeScope();
        }

        #endregion
    }
}