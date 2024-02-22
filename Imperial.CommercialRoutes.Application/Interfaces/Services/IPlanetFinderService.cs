using Imperial.CommercialRoutes.Domain.Entities;
using Imperial.CommercialRoutes.Domain.Entities.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Imperial.CommercialRoutes.Application.Interfaces.Services
{
    /// <summary>
    /// Service definition for planet finder.
    /// </summary>
    public interface IPlanetFinderService
    {
        /// <summary>
        /// Gets planets from internal data source. If there is no planet; searches from external source.
        /// </summary>
        /// <param name="options">Options to search specific planets.</param>
        /// <param name="cancellationToken">Used to propagate notifications that the operation should be canceled.</param>
        /// <returns>List of planets.</returns>
        Task<IList<Planet>> GetPlanetsAsync(
            PlanetSearchOptions options = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets planet from internal data source. If there is no planet; searches from external source.
        /// </summary>
        /// <param name="options">Options to search specific planet.</param>
        /// <param name="cancellationToken">Used to propagate notifications that the operation should be canceled.</param>
        /// <returns>Planet.</returns>
        Task<Planet> GetPlanetAsync(
            PlanetSearchOptions options = null,
            CancellationToken cancellationToken = default);
    }
}
