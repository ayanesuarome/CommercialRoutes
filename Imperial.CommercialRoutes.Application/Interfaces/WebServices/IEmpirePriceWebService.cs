using Imperial.CommercialRoutes.Application.DTOs;
using Imperial.CommercialRoutes.Domain.Entities.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Imperial.CommercialRoutes.Application.Interfaces.WebServices
{
    /// <summary>
    /// Service definition for empire price.
    /// </summary>
    public interface IEmpirePriceWebService
    {
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
        Task<IList<FuelPrice>> GetFuelPricesAsync(
            FuelPriceSearchOptions options = null,
            CancellationToken cancellationToken = default);
    }
}
