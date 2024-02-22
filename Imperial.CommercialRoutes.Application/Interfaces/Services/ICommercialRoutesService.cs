using Imperial.CommercialRoutes.Application.DTOs;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Imperial.CommercialRoutes.Application.Services
{
    public interface ICommercialRoutesService
    {
        /// <summary>
        /// Gets commercial routes.
        /// </summary>
        /// <remarks>
        /// Getting all the commercial routes of the imperial fleet galaxy is not scalable at all regardeless the current data set is short,
        /// millions of planets should exist and loading/storing all of them at once is not scalable at all and can lead into memory issues.
        /// </remarks>
        /// <param name="cancellationToken">Used to propagate notifications that the operation should be canceled.</param>
        /// <returns>List of commercial routes.</returns>
        Task<IList<CommercialRoute>> GetCommercialRoutesAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets route price breakdown.
        /// </summary>
        /// <param name="request">Route request.</param>
        /// <param name="cancellationToken">Used to propagate notifications that the operation should be canceled.</param>
        /// <returns>Route price breakdown.</returns>
        Task<BreakdownRoutePrice> GetRoutePriceBreakdown(
            RouteRequest request,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets optimal aircraft.
        /// </summary>
        /// <param name="request">Route request.</param>
        /// <param name="cancellationToken">Used to propagate notifications that the operation should be canceled.</param>
        /// <returns></returns>
        Task<AircraftResponse> GetOptimalAircraft(
            RouteRequest request,
            CancellationToken cancellationToken = default);
    }
}
