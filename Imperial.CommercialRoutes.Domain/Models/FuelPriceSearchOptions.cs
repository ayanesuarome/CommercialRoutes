namespace Imperial.CommercialRoutes.Domain.Entities.Models
{
    /// <summary>
    /// Options to search fuel prices.
    /// </summary>
    public class FuelPriceSearchOptions
    {
        /// <summary>
        /// Gets or sets the sector to search prices.
        /// </summary>
        public string Sector { get; set; }

        /// <summary>
        /// Gets or sets the price per lunar day to search prices.
        /// </summary>
        public decimal? PricePerLunarDay { get; set; }

        /// <summary>
        /// Gets or sets the day of week to search prices.
        /// </summary>
        public int? DayOfWeek { get; set; }
    }
}
