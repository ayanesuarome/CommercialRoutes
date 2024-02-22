using Autofac;
using RestSharp;
using System.Configuration;

namespace Imperial.CommercialRoutes.WebApi.IoC.Modules
{
    /// <summary>
    /// Autofac module to add the HTTP client.
    /// </summary>
    public class RestClientModule : Module
    {
        private const string VuelingBaseAddress = "VuelingBaseAddress";

        /// <summary>
        /// Adds registrations to the container.
        /// </summary>
        /// <param name="builder">The builder through which components can be registered.</param>
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(_ => new RestClient(ConfigurationManager.AppSettings.Get(VuelingBaseAddress)))
                .As<IRestClient>()
                .SingleInstance();

            builder.Register(_ => new RestClient(ConfigurationManager.AppSettings.Get(VuelingBaseAddress)))
                .As<RestClient>()
                .SingleInstance();
        }
    }
}
