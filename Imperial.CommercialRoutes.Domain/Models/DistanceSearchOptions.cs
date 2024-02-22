namespace Imperial.CommercialRoutes.Domain.Entities.Models
{
    /// <summary>
    /// Options to search distances.
    /// </summary>
    public class DistanceSearchOptions
    {
        /// <summary>
        /// Gets or sets the origin code of the distance to search for.
        /// </summary>
        public string Origin { get; set; }

        /// <summary>
        /// Gets or sets the destination code of the distance to search for.
        /// </summary>
        public string Destination { get; set; }

        /// <summary>
        /// Gets or sets the lunar years of the distance to search for.
        /// </summary>
        public decimal? LunarYears { get; set; }
    }
}
