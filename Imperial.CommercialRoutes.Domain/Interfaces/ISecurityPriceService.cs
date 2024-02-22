using Imperial.CommercialRoutes.Domain.Entities;

namespace Imperial.CommercialRoutes.Domain.Interfaces
{
    /// <summary>
    /// Service price calculator definition.
    /// </summary>
    public interface ISecurityPriceService
    {
        /// <summary>
        /// Calculates the tax "EliteStromTrooper".
        /// </summary>
        /// <param name="origin">Origin planet.</param>
        /// <param name="destination">Destination planet.</param>
        /// <returns>EliteStromTrooper.</returns>
        decimal CalculateEliteStromTrooperTax(Planet origin, Planet destination);

        /// <summary>
        /// Calculates the security price between the origin and destination planets.
        /// Calculates whether to apply the additonal tax "EliteStromTrooper".
        /// </summary>
        /// <param name="origin">Origin planet.</param>
        /// <param name="destination">Destination planet.</param>
        /// <returns>Security price.</returns>
        decimal CalculateSecurityPrice(Planet origin, Planet destination);
    }
}
