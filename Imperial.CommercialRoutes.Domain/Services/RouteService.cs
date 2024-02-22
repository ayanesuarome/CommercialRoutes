using Imperial.CommercialRoutes.Domain.Entities;
using Imperial.CommercialRoutes.Domain.Interfaces;
using System;

namespace Imperial.CommercialRoutes.Domain.Services
{
    /// <summary>
    /// Service route implementation.
    /// </summary>
    public class RouteService : IRouteService
    {
        /// <summary>
        /// Calculates the total rebel influence.
        /// </summary>
        /// <param name="origin">Origin planet.</param>
        /// <param name="destination">Destination planet.</param>
        /// <returns>Rebel influence for the route.</returns>
        public int CalculateRebelInfluence(Planet origin, Planet destination)
        {
            if (origin == null)
            {
                throw new ArgumentNullException(nameof(origin));
            }
            if (destination == null)
            {
                throw new ArgumentNullException(nameof(destination));
            }

            return origin.RebelInfluence + destination.RebelInfluence;
        }
    }
}
