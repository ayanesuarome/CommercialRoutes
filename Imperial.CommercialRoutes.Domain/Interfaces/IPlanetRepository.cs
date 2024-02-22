using Imperial.CommercialRoutes.Domain.Entities;
using Imperial.CommercialRoutes.Domain.Entities.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Imperial.CommercialRoutes.Domain.Interfaces
{
    /// <summary>
    /// Planet repository definition.
    /// </summary>
    public interface IPlanetRepository : ICreate<Planet>
    {
        /// <summary>
        /// Gets planet by query.
        /// </summary>
        /// <param name="options">Options to search specific planet.</param>
        /// <param name="cancellationToken">Used to propagate notifications that the operation should be canceled.</param>
        /// <returns>Planet.</returns>
        Task<Planet> GetPlanetAsync(
            PlanetSearchOptions options = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets planets by query.
        /// </summary>
        /// <param name="options">Options to search specific planets.</param>
        /// <param name="cancellationToken">Used to propagate notifications that the operation should be canceled.</param>
        /// <returns>List of planets.</returns>
        Task<IList<Planet>> GetPlanetsAsync(
            PlanetSearchOptions options = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets planets by names.
        /// </summary>
        /// <param name="names">Names of planets to search for.</param>
        /// <param name="cancellationToken">Used to propagate notifications that the operation should be canceled.</param>
        /// <returns>List of planets matching the provided names.</returns>
        Task<IList<Planet>> GetPlanetsByNamesAsync(
            string[] names,
            CancellationToken cancellationToken = default);
    }
}
