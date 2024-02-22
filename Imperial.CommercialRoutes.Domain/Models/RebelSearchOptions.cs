namespace Imperial.CommercialRoutes.Domain.Entities.Models
{
    /// <summary>
    /// Options to search rebel influence in planets.
    /// </summary>
    public class RebelSearchOptions
    {
        /// <summary>
        /// Gets or sets the planet code to search rebel influence.
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets the rebel influence value to search rebel influences.
        /// </summary>
        public int? RebelInfluence { get; set; }
    }
}
