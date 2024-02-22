namespace Imperial.CommercialRoutes.Application.DTOs
{
    /// <summary>
    /// Represents a distance tax.
    /// </summary>
    public class RouteTax
    {
        /// <summary>
        /// Gets or sets the defense cost of the origin planet.
        /// </summary>
        public decimal OriginDefenseCost { get; set; }

        /// <summary>
        /// Gets or sets the defense cost of the destination planet.
        /// </summary>
        public decimal DestinationDefenseCost { get; set; }

        /// <summary>
        /// Gets or sets the elite strom trooper cost.
        /// </summary>
        public decimal EliteDefenseCost { get; set; }
    }
}
