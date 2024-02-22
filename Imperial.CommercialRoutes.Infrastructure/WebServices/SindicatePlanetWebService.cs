using Imperial.CommercialRoutes.Application.DTOs;
using Imperial.CommercialRoutes.Application.Interfaces.WebServices;
using Imperial.CommercialRoutes.Domain.Entities.Models;
using RestSharp;
using Serilog;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Imperial.CommercialRoutes.Infrastructure.WebServices
{
    /// <summary>
    /// Service implementation for sindicate planet.
    /// </summary>
    public class SindicatePlanetWebService : BaseWebService, ISindicatePlanetWebService
    {
        #region Fields

        private string SindicatePlanetsAddress => ConfigurationManager.AppSettings.Get("SindicatePlanetsAddress");
        
        #endregion

        #region Constructor
        
        /// <summary>
        /// Initializes a new instance of the class <see cref="SindicatePlanetWebService"/>.
        /// </summary>
        /// <param name="logger">Logger instance.</param>
        /// <param name="client">HTTP client instance.</param>
        public SindicatePlanetWebService(
            ILogger logger,
            IRestClient client)
            : base(logger, client)
        {
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets list of planets.
        /// </summary>
        /// <param name="options">Options to search specific planets.</param>
        /// <param name="cancellationToken">Used to propagate notifications that the operation should be canceled.</param>
        /// <returns>List of planets</returns>
        public async Task<IList<SindicatePlanet>> GetPlanetsAsync(
            PlanetSearchOptions options = null,
            CancellationToken cancellationToken = default)
        {
            RestRequest request = GetRestRequest(SindicatePlanetsAddress, Method.Get);
            IEnumerable<SindicatePlanet> response = await ExecuteAsync<IEnumerable<SindicatePlanet>>(request, cancellationToken);

            // Applying search filter to simulate that the service supports OData or any other filtering support.
            // This should not be here if the service has supported filtering.
            return (
                        from planet in response
                        where (options?.Name == null || planet.Name == options.Name)
                        && (options?.Code == null || planet.Code == options.Code)
                        && (options?.Sector == null || planet.Sector == options.Sector)
                        select planet
                    )
                    .ToList();
        }

        #endregion
    }
}
