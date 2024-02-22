namespace Imperial.CommercialRoutes.Application.DTOs
{
    /// <summary>
    /// Represents a breakdown route price.
    /// </summary>
    public class BreakdownRoutePrice
    {
        /// <summary>
        /// Gets or sets the total price of the route.
        /// </summary>
        public decimal TotalAmount { get; set; }

        /// <summary>
        /// Gets or sets the price per lunar day of the route.
        /// </summary>
        public decimal PricePerLunarDay { get; set; }

        /// <summary>
        /// Gets or sets the route tax to applied to the route.
        /// </summary>
        public RouteTax Tax { get; set; }
    }
}