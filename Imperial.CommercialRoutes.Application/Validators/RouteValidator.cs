using FluentValidation;
using Imperial.CommercialRoutes.Application.DTOs;

namespace Imperial.CommercialRoutes.Application.Validators
{
    /// <summary>
    /// Validator rules for <see cref="RouteRequest"/>.
    /// </summary>
    public class RouteRequestValidator : AbstractValidator<RouteRequest>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RouteRequestValidator"/> class.
        /// </summary>
        public RouteRequestValidator()
        {
            RuleFor(m => m.Origin)
                .NotEmpty()
                .WithMessage($"{nameof(RouteRequest.Origin)} is required");

            RuleFor(m => m.Destination)
                .NotEmpty()
                .WithMessage($"{nameof(RouteRequest.Destination)} is required");

            RuleFor(m => m.DayOfWeek)
                .InclusiveBetween(0, 6)
                .WithMessage($"{nameof(RouteRequest.DayOfWeek)} must be between 0-6")
                .When(m => m.DayOfWeek.HasValue);
        }
    }
}
