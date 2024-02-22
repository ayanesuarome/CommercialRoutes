using Imperial.CommercialRoutes.Application.DTOs;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Imperial.CommercialRoutes.Application.Interfaces.WebServices
{
    /// <summary>
    /// Service definition for sindicate distance.
    /// </summary>
    public interface ISindicateDistanceWebService
    {
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
        Task<IDictionary<string, IEnumerable<SindicateDistance>>> GetDistancesAsync(
            string origin,
            string destination,
            CancellationToken cancellationToken = default);
    }
}
