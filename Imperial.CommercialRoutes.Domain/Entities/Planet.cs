using Dapper.Contrib.Extensions;

namespace Imperial.CommercialRoutes.Domain.Entities
{
    /// <summary>
    /// Represents a planet.
    /// </summary>
    [Table("Planet")]
    public class Planet : Entity
    {
        /// <summary>
        /// Gets or sets name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets code.
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets sector.
        /// </summary>
        public string Sector { get; set; }

        /// <summary>
        /// Gets or sets rebel influence.
        /// </summary>
        public int RebelInfluence { get; set; }

        /// <summary>
        /// Gets the rebel influence defense cost.
        /// </summary>
        [Write(false)]
        public decimal DefenseCost => RebelInfluence / 100m;
    }
}
