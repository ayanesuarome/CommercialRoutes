using Imperial.CommercialRoutes.Application.DTOs;
using Imperial.CommercialRoutes.Application.Interfaces.WebServices;
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
    /// Service implementation for sindicate distance service.
    /// </summary>
    public class SindicateDistanceWebService : BaseWebService, ISindicateDistanceWebService
    {
        #region Fields

        private string SindicateDistancesAddress => ConfigurationManager.AppSettings.Get("SindicateDistancesAddress");
        
        #endregion

        #region Constructor
        
        /// <summary>
        /// Initializes a new instance of the class <see cref="SindicateDistanceWebService"/>.
        /// </summary>
        /// <param name="logger">Logger instance.</param>
        /// <param name="client">HTTP client instance.</param>
        public SindicateDistanceWebService(
            ILogger logger,
            IRestClient client)
            : base(logger, client)
        {
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets list of distances.
        /// </summary>
        /// <remarks>
        /// Parameters <paramref name="origin"/> and <paramref name="destination"/> are here only to simulate a scalable search filter
        /// because regardeless the current data set is short, millions of distances should exist
        /// and loading all of them at once is not scalable at all.
        /// </remarks>
        /// <param name="origin">Origin of the distance to search for.</param>
        /// <param name="destination">Destination of the distance to search for.</param>
        /// <param name="cancellationToken">Used to propagate notifications that the operation should be canceled.</param>
        /// <returns>Dictionary with origin key and pair of destination and lunar years as value.</returns>
        public async Task<IDictionary<string, IEnumerable<SindicateDistance>>> GetDistancesAsync(
            string origin,
            string destination,
            CancellationToken cancellationToken = default)
        {
            RestRequest request = GetRestRequest(SindicateDistancesAddress, Method.Get);
            var response = await ExecuteAsync<IDictionary<string, IEnumerable<SindicateDistance>>>(request, cancellationToken);

            // Applying search filter to simulate that the service supports OData or any other filtering support.
            // This should not be here if the service has supported filtering.
            // For the sake of test, only origin filtered is supported.
            var distanceFiltered = new Dictionary<string, IEnumerable<SindicateDistance>>();

            if (origin != null && response.ContainsKey(origin))
            {
                distanceFiltered.Add(origin, response[origin]);
                if (destination != null)
                {
                    distanceFiltered[origin] = distanceFiltered[origin].Where(d => d.Code == destination);
                }

                response = distanceFiltered;
            }

            return response;
        }

        #endregion
    }
}
