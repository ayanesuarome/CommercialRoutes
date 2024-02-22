using AutoMapper;
using Imperial.CommercialRoutes.Application.AutoMapper;
using System;
using System.Collections.Generic;
using Xunit;

namespace Imperial.CommercialRoutes.Application.Tests.AutoMapper
{
    /// <summary>
    /// Test class to test the mapper configuration.
    /// </summary>
    public class MapperConfigurationTests
    {
        #region Test Methods

        /// <summary>
        /// Tests the mapper profiles configuration.
        /// </summary>
        /// <param name="profileType">Profile type.</param>
        [Theory, MemberData(nameof(GetMapperProfiles))]
        public void TestAutoMapperProfilesConfiguration(Type profileType)
        {
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(profileType));

            configuration.AssertConfigurationIsValid();
        }

        #endregion

        #region Member Test Data

        public static IEnumerable<object[]> GetMapperProfiles()
        {
            yield return new object[] { typeof(RouteToCommercialRouteProfile) };
            yield return new object[] { typeof(SindicateDistanceToRouteProfile) };
            yield return new object[] { typeof(SindicatePlanetToPlanetProfile) };
            yield return new object[] { typeof(AircraftToAircraftResponse) };
        }

        #endregion
    }
}
