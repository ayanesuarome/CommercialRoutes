using Imperial.CommercialRoutes.Application.DTOs;
using Imperial.CommercialRoutes.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace Imperial.CommercialRoutes.Application.Interfaces.Services
{
    /// <summary>
    /// Route price breakdown service definition.
    /// </summary>
    public interface IBreakDownRoutePriceService
    {
        /// <summary>
        /// Calculates the route price breakdown including taxes.
        /// </summary>
        /// <param name="dayOfWeek">Day of week.</param>
        /// <param name="origin">Origin planet</param>
        /// <param name="destination">Destination planet</param>
        /// <param name="distance">Distance.</param>
        /// <param name="cancellationToken">Used to propagate notifications that the operation should be canceled.</param>
        /// <returns>Route price breakdown</returns>
        Task<BreakdownRoutePrice> CalculateBreakdownRoutePrice(
            int dayOfWeek,
            Planet origin,
            Planet destination,
            Distance distance,
            CancellationToken cancellationToken = default);
    }
}
