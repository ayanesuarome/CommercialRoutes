using Imperial.CommercialRoutes.Domain.Entities;
using Imperial.CommercialRoutes.Domain.Entities.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Imperial.CommercialRoutes.Application.Interfaces.Services
{
    /// <summary>
    /// Service definition for distance finder.
    /// </summary>
    public interface IDistanceFinderService
    {
        /// <summary>
        /// Gets list of distances from internal data source. If there is no distance; searches from external source.
        /// </summary>
        /// <param name="options">Options to search specific distances.</param>
        /// <param name="cancellationToken">Used to propagate notifications that the operation should be canceled.</param>
        /// <returns>List of distances.</returns>
        Task<IList<Distance>> GetDistancesAsync(
            DistanceSearchOptions options = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets distance from internal data source. If there is no distance; searches from external source.
        /// </summary>
        /// <param name="origin">Origin of the distance to search for.</param>
        /// <param name="destination">Destination of the distance to search for.</param>
        /// <param name="cancellationToken">Used to propagate notifications that the operation should be canceled.</param>
        /// <returns>Distances.</returns>
        Task<Distance> GetDistanceAsync(
            string origin,
            string destination,
            CancellationToken cancellationToken = default);
    }
}
