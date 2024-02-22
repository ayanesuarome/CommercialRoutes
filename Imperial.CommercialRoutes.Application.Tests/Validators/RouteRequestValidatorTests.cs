using FluentValidation.TestHelper;
using Imperial.CommercialRoutes.Application.DTOs;
using Imperial.CommercialRoutes.Application.Validators;
using System;
using Xunit;

namespace Imperial.CommercialRoutes.Application.Tests.Validators
{
    /// <summary>
    /// Tests class to tests validator <see cref="RouteRequestValidator"/>.
    /// </summary>
    public class RouteRequestValidatorTests : IDisposable
    {
        #region Fields

        private RouteRequestValidator validator;

        #endregion

        #region Setup and Cleanup

        /// <summary>
        /// Initializes a new instance of the class <see cref="RouteRequestValidatorTests"/>.
        /// </summary>
        public RouteRequestValidatorTests() {
            validator = new RouteRequestValidator();
        }

        public void Dispose()
        {
            validator = null;
        }

        #endregion

        #region Test Methods

        /// <summary>
        /// Tests that <see cref="RouteRequestValidator"/> should fail with required properties:
        /// <see cref="RouteRequest.Origin"/>
        /// <see cref="RouteRequest.Destination"/>
        /// </summary>
        [Fact]
        public void TestValidatorShouldFailForRequiredProperties()
        {
            var request = new RouteRequest();

            var result = validator.TestValidate(request);

            result.ShouldHaveValidationErrorFor(x => x.Origin)
                .WithErrorMessage($"{nameof(RouteRequest.Origin)} is required");
            result.ShouldHaveValidationErrorFor(x => x.Destination)
                .WithErrorMessage($"{nameof(RouteRequest.Destination)} is required");
        }

        /// <summary>
        /// Tests that <see cref="RouteRequestValidator"/> should fail with <see cref="RouteRequest.DayOfWeek"/> out of range 0-6.
        /// </summary>
        [Fact]
        public void TestValidatorShouldFailForDayOfWeekOutOfRangeProperties()
        {
            var request = new RouteRequest
            {
                Origin = "origin",
                Destination = "destination",
                DayOfWeek = 7
            };

            var result = validator.TestValidate(request);

            result.ShouldNotHaveValidationErrorFor(x => x.Origin);
            result.ShouldNotHaveValidationErrorFor(x => x.Destination);
            result.ShouldHaveValidationErrorFor(x => x.DayOfWeek)
                .WithErrorMessage($"{nameof(RouteRequest.DayOfWeek)} must be between 0-6");
        }

        /// <summary>
        /// Tests that <see cref="RouteRequestValidator"/> should not fail.
        /// </summary>
        [Fact]
        public void TestValidatorShouldNotFail()
        {
            var request = new RouteRequest
            {
                Origin = "origin",
                Destination = "destination",
            };

            var result = validator.TestValidate(request);

            result.ShouldNotHaveValidationErrorFor(x => x.Origin);
            result.ShouldNotHaveValidationErrorFor(x => x.Destination);
            result.ShouldNotHaveValidationErrorFor(x => x.DayOfWeek);
        }

        #endregion
    }
}
