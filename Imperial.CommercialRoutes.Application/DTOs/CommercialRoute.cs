namespace Imperial.CommercialRoutes.Application.DTOs
{
    /// <summary>
    /// Commercial route.
    /// </summary>
    public class CommercialRoute
    {
        /// <summary>
        /// Gets or sets origin.
        /// </summary>
        public string Origin { get; set; }

        /// <summary>
        /// Gets or sets destination.
        /// </summary>
        public string Destination { get; set; }

        /// <summary>
        /// Gets or sets distance.
        /// </summary>
        public decimal Distance { get; set; }
    }
}
