using Imperial.CommercialRoutes.Application.DTOs;
using Imperial.CommercialRoutes.Domain.Entities.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Imperial.CommercialRoutes.Application.Interfaces.WebServices
{
    /// <summary>
    /// Service definition for sindicate planet.
    /// </summary>
    public interface ISindicatePlanetWebService
    {
        /// <summary>
        /// Gets list of planets.
        /// </summary>
        /// <param name="options">Options to search specific planets.</param>
        /// <param name="cancellationToken">Used to propagate notifications that the operation should be canceled.</param>
        /// <returns>List of planets.</returns>
        Task<IList<SindicatePlanet>> GetPlanetsAsync(
            PlanetSearchOptions options = null,
            CancellationToken cancellationToken = default);
    }
}
