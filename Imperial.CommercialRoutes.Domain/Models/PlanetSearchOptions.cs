namespace Imperial.CommercialRoutes.Domain.Entities.Models
{
    /// <summary>
    /// Options to search planets.
    /// </summary>
    public class PlanetSearchOptions
    {
        /// <summary>
        /// Gets or sets the name to search planets.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the code to search planets.
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets the sector to search planets.
        /// </summary>
        public string Sector { get; set; }

        /// <summary>
        /// Gets or sets rebel influence to search planets.
        /// </summary>
        public int? RebelInfluence { get; set; }
    }
}
