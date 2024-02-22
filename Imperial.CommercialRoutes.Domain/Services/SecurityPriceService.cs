using Imperial.CommercialRoutes.Domain.Entities;
using Imperial.CommercialRoutes.Domain.Interfaces;
using System;

namespace Imperial.CommercialRoutes.Domain.Services
{
    /// <summary>
    /// Service price calculator implementation.
    /// </summary>
    public class SecurityPriceService : ISecurityPriceService
    {
        /// <summary>
        /// Calculates the tax "EliteStromTrooper".
        /// </summary>
        /// <param name="origin">Origin planet.</param>
        /// <param name="destination">Destination planet.</param>
        /// <returns>EliteStromTrooper.</returns>
        public decimal CalculateEliteStromTrooperTax(Planet origin, Planet destination)
        {
            if(origin == null)
            {
                throw new ArgumentNullException(nameof(origin));
            }
            if (destination == null)
            {
                throw new ArgumentNullException(nameof(destination));
            }

            decimal eliteDefenseCost = 0m;

            if (origin.DefenseCost + destination.DefenseCost > 0.4m)
            {
                eliteDefenseCost = (origin.DefenseCost + destination.DefenseCost) - 0.4m;
            }
            
            return eliteDefenseCost;
        }

        /// <summary>
        /// Calculates the security price between the origin and destination planets.
        /// </summary>
        /// <param name="origin">Origin planet.</param>
        /// <param name="destination">Destination planet.</param>
        /// <returns>Security price.</returns>
        public decimal CalculateSecurityPrice(Planet origin, Planet destination)
        {
            if (origin == null)
            {
                throw new ArgumentNullException(nameof(origin));
            }
            if (destination == null)
            {
                throw new ArgumentNullException(nameof(destination));
            }

            return origin.DefenseCost + destination.DefenseCost + CalculateEliteStromTrooperTax(origin, destination);
        }
    }
}
