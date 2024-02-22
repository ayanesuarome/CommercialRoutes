using Dapper.Contrib.Extensions;

namespace Imperial.CommercialRoutes.Domain.Entities
{
    /// <summary>
    /// Represents a distance.
    /// </summary>
    [Table("Distance")]
    public class Distance : Entity
    {
        /// <summary>
        /// Gets or sets the planet origin code.
        /// </summary>
        public string Origin { get; set; }

        /// <summary>
        /// Gets or sets the planet destination code.
        /// </summary>
        public string Destination { get; set; }

        /// <summary>
        /// Gets or sets the lunar years between the two origin and destination planets.
        /// </summary>
        public decimal LunarYears { get; set; }

        /// <summary>
        /// Gets distance in lunar days.
        /// </summary>
        [Write(false)]
        public decimal LunarDays => LunarYears * 365;
    }
}
