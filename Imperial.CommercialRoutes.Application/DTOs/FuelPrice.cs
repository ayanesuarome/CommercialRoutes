using System.Text.Json.Serialization;

namespace Imperial.CommercialRoutes.Application.DTOs
{
    /// <summary>
    /// Represents a fuel price per light days.
    /// </summary>
    public class FuelPrice
    {
        /// <summary>
        /// Gets or sets the sector where the planet belongs to.
        /// </summary>
        public string Sector { get; set; }

        /// <summary>
        /// Gets or sets the price per lunar day.
        /// </summary>
        [JsonPropertyName("PricesPerLunarDay")]
        public decimal PricePerLunarDay { get; set; }

        /// <summary>
        /// Gets or sets the day of week for the current price.
        /// </summary>
        [JsonPropertyName("DayOfTheWeek")]
        public int DayOfWeek { get; set; }
    }
}
