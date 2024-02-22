using Autofac;
using Imperial.CommercialRoutes.Infrastructure.DatabaseContext;
using Serilog.Core;
using Serilog;
using System;

namespace Imperial.CommercialRoutes.WebApi.IoC.Modules
{
    /// <summary>
    /// Autofac module to add DB context.
    /// </summary>
    public class DbContextModule : Module
    {
        private const string ConnectionName = "CommercialRoutesDbConnection";
        private readonly Logger logger = new LoggerConfiguration().ReadFrom.AppSettings().CreateLogger();

        /// <summary>
        /// Adds registrations to the container.
        /// </summary>
        /// <param name="builder">The builder through which components can be registered.</param>
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register<CommercialRoutesDbContext>(ctx =>
            {
                try
                {
                    CommercialRoutesDbContext context = new CommercialRoutesDbContext(ConnectionName);
                    context.Init();

                    return context;
                }
                catch (Exception ex)
                {
                    logger.Error("Failure during DbContext registration with error: {0}", ex.Message);
                    return null;
                }
            }).InstancePerLifetimeScope();
        }
    }
}
