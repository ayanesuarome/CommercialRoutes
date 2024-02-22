using System.Text.Json.Serialization;

namespace Imperial.CommercialRoutes.Application.DTOs
{
    /// <summary>
    /// Represents the sindicate planet.
    /// </summary>
    public class SindicatePlanet
    {
        /// <summary>
        /// Gets or sets name.
        /// </summary>
        [JsonPropertyName("PlanetName")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets code.
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets sector.
        /// </summary>
        public string Sector { get; set; }
    }
}
