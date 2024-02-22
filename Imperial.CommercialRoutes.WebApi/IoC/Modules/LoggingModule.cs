using Autofac;
using Imperial.CommercialRoutes.Infrastructure.Logging;
using Serilog;
using Serilog.Core;
using System;
using System.Linq;

namespace Imperial.CommercialRoutes.WebApi.IoC.Modules
{
    /// <summary>
    /// Autofac module to add logging.
    /// </summary>
    public class LoggingModule : Module
    {
        private const string TargetTypeParameterName = "Autofac.AutowiringPropertyInjector.InstanceType";

        /// <summary>
        /// Adds registrations to the container.
        /// </summary>
        /// <param name="builder">The builder through which components can be registered.</param>
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(_ =>
            {
                Logger logger = new LoggerConfiguration().ReadFrom.AppSettings().CreateLogger();
                LoggerProvider provider = new LoggerProvider(logger);
                return provider;
            })
                .AsSelf()
                .AutoActivate()
                .SingleInstance();

            builder.Register((c, p) =>
            {
                ILogger logger = c.Resolve<LoggerProvider>().GetLogger();

                NamedParameter targetType = p.OfType<NamedParameter>()
                    .FirstOrDefault(np => np.Name == TargetTypeParameterName && np.Value is Type);

                if (targetType != null)
                {
                    return logger.ForContext((Type)targetType.Value);
                }

                return logger;
            })
                .As<ILogger>()
                .ExternallyOwned();
        }
    }
}