using Imperial.CommercialRoutes.Domain.Entities;
using Imperial.CommercialRoutes.Domain.Entities.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Imperial.CommercialRoutes.Domain.Interfaces
{
    /// <summary>
    /// Distance repository definition.
    /// </summary>
    public interface IDistanceRepository : ICreate<Distance>
    {
        /// <summary>
        /// Gets distance by query.
        /// </summary>
        /// <param name="options">Options to search specific distance.</param>
        /// <param name="cancellationToken">Used to propagate notifications that the operation should be canceled.</param>
        /// <returns>Distance.</returns>
        Task<Distance> GetDistanceAsync(
            DistanceSearchOptions options = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets distances by query.
        /// </summary>
        /// <param name="options">Options to search specific distances.</param>
        /// <param name="cancellationToken">Used to propagate notifications that the operation should be canceled.</param>
        /// <returns>List of distances.</returns>
        Task<IList<Distance>> GetDistancesAsync(
            DistanceSearchOptions options = null,
            CancellationToken cancellationToken = default);
    }
}
