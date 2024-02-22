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
    public class EmpirePriceWebService : BaseWebService, IEmpirePriceWebService
    {
        #region Fields

        private string EmpirePricesAddress => ConfigurationManager.AppSettings.Get("EmpirePricesAddress");

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the class <see cref="EmpirePriceWebService"/>.
        /// </summary>
        /// <param name="logger">Logger instance.</param>
        /// <param name="client">HTTP client instance.</param>
        public EmpirePriceWebService(
            ILogger logger,
            IRestClient client)
            : base(logger, client)
        {
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets fuel prices per sector and lunar day.
        /// </summary>
        /// <remarks>
        /// Parameter option is here only to simulate a scalable search filter because regardeless the current data set is short,
        /// millions of prices per sector should exist and loading all of them at once is not scalable at all.
        /// </remarks>
        /// <param name="options">Options to search specific price.</param>
        /// <param name="cancellationToken">Used to propagate notifications that the operation should be canceled.</param>
        /// <returns>List of prices.</returns>
        public async Task<IList<FuelPrice>> GetFuelPricesAsync(
            FuelPriceSearchOptions options = null,
            CancellationToken cancellationToken = default)
        {
            RestRequest request = GetRestRequest(EmpirePricesAddress, Method.Get);
            IEnumerable<FuelPrice> response = await ExecuteAsync<IEnumerable<FuelPrice>>(request, cancellationToken);

            // Applying search filter to simulate that the service supports OData or any other filtering support.
            // This should not be here if the service has supported filtering.
            return (
                        from price in response
                        where (options?.Sector == null || price.Sector == options.Sector)
                        && (options?.PricePerLunarDay == null || price.PricePerLunarDay == options.PricePerLunarDay)
                        && (options?.DayOfWeek == null || price.DayOfWeek == options.DayOfWeek)
                        select price
                    )
                    .ToList();
        }

        #endregion
    }
}