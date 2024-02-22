using Imperial.CommercialRoutes.Domain.Entities;

namespace Imperial.CommercialRoutes.Domain.Interfaces
{
    /// <summary>
    /// Route service definition.
    /// </summary>
    public interface IRouteService
    {
        /// <summary>
        /// Calculates the total rebel influence.
        /// </summary>
        /// <param name="origin">Origin planet.</param>
        /// <param name="destination">Destination planet.</param>
        /// <returns>Rebel influence for the route.</returns>
        int CalculateRebelInfluence(Planet origin, Planet destination);
    }
}
