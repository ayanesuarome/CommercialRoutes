using FluentValidation.Attributes;
using Imperial.CommercialRoutes.Application.Validators;

namespace Imperial.CommercialRoutes.Application.DTOs
{
    /// <summary>
    /// Represents a distance request.
    /// </summary>
    [Validator(typeof(RouteRequestValidator))]
    public class RouteRequest
    {
        /// <summary>
        /// Gets or sets the origin planet name.
        /// </summary>
        public string Origin { get; set; }

        /// <summary>
        /// Gets or sets the destination planet name.
        /// </summary>
        public string Destination { get; set; }

        /// <summary>
        /// Gets or sets day of week.
        /// </summary>
        public int? DayOfWeek { get; set; }
    }
}
