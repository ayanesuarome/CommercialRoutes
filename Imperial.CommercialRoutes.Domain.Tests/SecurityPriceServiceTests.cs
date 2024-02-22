using FluentAssertions;
using Imperial.CommercialRoutes.Domain.Entities;
using Imperial.CommercialRoutes.Domain.Interfaces;
using Imperial.CommercialRoutes.Domain.Services;
using System;
using System.Collections.Generic;
using Xunit;

namespace Imperial.CommercialRoutes.Domain.Tests
{
    /// <summary>
    /// Tests class to tests the <see cref="ISecurityPriceService"/> implementations.
    /// </summary>
    public class SecurityPriceServiceTests : IDisposable
    {
        #region Fields

        private ISecurityPriceService _service;

        #endregion

        #region Setup and Cleanup

        /// <summary>
        /// Initializes a new instance of the class <see cref="SecurityPriceServiceTests"/>.
        /// </summary>
        public SecurityPriceServiceTests()
        {
            _service = new SecurityPriceService();
        }

        /// <summary>
        /// Releases unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            _service = null;
        }

        #endregion

        #region Tests Methods

        /// <summary>
        /// Tests that <see cref="ISecurityPriceService.CalculateEliteStromTrooperTax(Planet, Planet)"/>
        /// throws <see cref="ArgumentNullException"/> for null inputs.
        /// </summary>
        [Theory, MemberData(nameof(GetTestNullArguments))]
        public void TestCalculateEliteStromTrooperTaxThrowsArgumentNullExceptionWithNullInputs(Planet origin, Planet destination)
        {
            Assert.Throws<ArgumentNullException>(() => _service.CalculateEliteStromTrooperTax(origin, destination));
        }

        /// <summary>
        /// Tests that <see cref="ISecurityPriceService.CalculateSecurityPrice(Planet, Planet)"/>
        /// throws <see cref="ArgumentNullException"/> for null inputs.
        /// </summary>
        [Theory, MemberData(nameof(GetTestNullArguments))]
        public void TestCalculateSecurityPriceThrowsArgumentNullExceptionWithNullInputs(Planet origin, Planet destination)
        {
            Assert.Throws<ArgumentNullException>(() => _service.CalculateSecurityPrice(origin, destination));
        }

        /// <summary>
        /// Tests that <see cref="ISecurityPriceService.CalculateEliteStromTrooperTax(Planet, Planet)"/> should return zero.
        /// </summary>
        [Fact]
        public void TestCalculateEliteStromTrooperTaxReturnZero()
        {
            Planet origin = new Planet
            {
                RebelInfluence = 10
            };
            Planet destination = new Planet
            {
                RebelInfluence = 20
            };

            decimal eliteDefenseCost = _service.CalculateEliteStromTrooperTax(origin, destination);

            eliteDefenseCost.Should().Be(0);
        }
        
        /// <summary>
        /// Tests that <see cref="ISecurityPriceService.CalculateEliteStromTrooperTax(Planet, Planet)"/> should return a value greather than zero.
        /// </summary>
        [Fact]
        public void TestCalculateEliteStromTrooperTaxReturnValueGreatherThanZero()
        {
            Planet origin = new Planet
            {
                RebelInfluence = 21
            };
            Planet destination = new Planet
            {
                RebelInfluence = 20
            };

            decimal eliteDefenseCost = _service.CalculateEliteStromTrooperTax(origin, destination);

            eliteDefenseCost.Should().BeGreaterThan(0);
        }

        /// <summary>
        /// Tests that <see cref="ISecurityPriceService.CalculateSecurityPrice(Planet, Planet)"/>
        /// should not apply the EliteStromTrooper tax.
        /// </summary>
        [Fact]
        public void TestCalculateSecurityPriceNotApplyEliteStromTrooperTax()
        {
            Planet origin = new Planet
            {
                RebelInfluence = 10
            };
            Planet destination = new Planet
            {
                RebelInfluence = 20
            };

            decimal price = _service.CalculateSecurityPrice(origin, destination);

            price.Should().Be(0.3m);
        }
        
        /// <summary>
        /// Tests that <see cref="ISecurityPriceService.CalculateSecurityPrice(Planet, Planet)"/>
        /// should apply the EliteStromTrooper tax.
        /// </summary>
        [Fact]
        public void TestCalculateSecurityPriceApplyEliteStromTrooperTax()
        {
            Planet origin = new Planet
            {
                RebelInfluence = 40
            };
            Planet destination = new Planet
            {
                RebelInfluence = 20
            };

            decimal eliteDefenseCost = (origin.DefenseCost + destination.DefenseCost) - 0.4m;
            decimal price = _service.CalculateSecurityPrice(origin, destination);

            price.Should().Be(0.6m + eliteDefenseCost);
        }

        #endregion

        #region Member Test Data

        public static IEnumerable<object[]> GetTestNullArguments()
        {
            yield return new object[] { null, new Planet() };
            yield return new object[] { new Planet(), null };
        }

        #endregion
    }
}
