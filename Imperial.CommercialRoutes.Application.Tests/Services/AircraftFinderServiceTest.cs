using FluentAssertions;
using Imperial.CommercialRoutes.Application.DTOs;
using Imperial.CommercialRoutes.Application.Interfaces.Services;
using Imperial.CommercialRoutes.Application.Interfaces.WebServices;
using Imperial.CommercialRoutes.Application.Services;
using Imperial.CommercialRoutes.Domain.Entities;
using Imperial.CommercialRoutes.Domain.Interfaces;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;

namespace Imperial.CommercialRoutes.Application.Tests.Services
{
    public class AircraftFinderServiceTest : IDisposable
    {
        #region Fields

        private IAircraftFinderService service;
        private Mock<IEmpireAircraftWebService> webServiceMock;
        private Mock<IRouteService> routeServiceMock;

        #endregion

        #region Setup and Cleanup

        /// <summary>
        /// Initializes a new instance of the class <see cref="AircraftFinderServiceTest"/>.
        /// </summary>
        public AircraftFinderServiceTest()
        {
            webServiceMock = new Mock<IEmpireAircraftWebService>();
            routeServiceMock = new Mock<IRouteService>();
            service = new AircraftFinderService(webServiceMock.Object, routeServiceMock.Object);
        }

        /// <summary>
        /// Releases unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            webServiceMock = null;
            routeServiceMock = null;
            service = null;
        }

        #endregion

        #region Tests Methods

        /// <summary>
        /// Test that <see cref="IAircraftFinderService.GetOptimalAircraft(Planet, Planet, Distance)"/>
        /// throws <see cref="ArgumentNullException"/> for null inputs.
        /// </summary>
        [Theory, MemberData(nameof(GetTestNullArguments))]
        public void TaskGetOptimalAircraftThrowsArgumentNullExceptionForNullInputs(Planet origin, Planet destination, Distance distance)
        {
            Assert.Throws<ArgumentNullException>(() =>  service.GetOptimalAircraft(origin, destination, distance));
        }

        /// <summary>
        /// Test that <see cref="IAircraftFinderService.GetOptimalAircraft(Planet, Planet, Distance)"/>
        /// retuns null when the service returns null or empty aircrafts/aircraft types.
        /// </summary>
        [Theory, MemberData(nameof(GetTestNullEmptyAircraftValues))]
        public void TaskGetOptimalAircraftReturnNull(EmpireAircraft aircraftMocked)
        {
            IAircraftFinderService service = SetupWebServiceMockToReturnTheGivenValues(aircraftMocked)
                .Build();

            EmpireAircraft.Aircraft aircraft  = service.GetOptimalAircraft(new Planet(), new Planet(), new Distance());

            aircraft.Should().BeNull();
        }

        /// <summary>
        /// Test that <see cref="IAircraftFinderService.GetOptimalAircraft(Planet, Planet, Distance)"/> retuns optimal aircraft.
        /// </summary>
        [Fact]
        public void TaskGetOptimalAircraftReturnOptimalAircraft()
        {
            Planet origin = new Planet
            {
                RebelInfluence = 40,
                Sector = "1A"
            };
            Planet destination = new Planet
            {
                RebelInfluence = 40,
            };

            IAircraftFinderService service = SetupRouteServiceMockToReturnTotalRebelInfluence(origin, destination)
                .SetupWebServiceMockToReturnAircrafts()
                .Build();

            Distance distance = new Distance
            {
                Origin = origin.Code,
                Destination = destination.Code,
                LunarYears = 70
            };

            EmpireAircraft.Aircraft aircraft = service.GetOptimalAircraft(origin, destination, distance);

            aircraft.Should().NotBeNull();
            aircraft.Type.Should().Be("cruise");
            aircraft.Reference.Should().Be(new Guid("8601b569-e9d6-440f-b160-4b9e1d030088"));
            aircraft.Sector.Should().Be("1A");
        }

        #endregion

        #region Member Test Data

        public static IEnumerable<object[]> GetTestNullArguments()
        {
            yield return new object[] { null, new Planet(), new Distance() };
            yield return new object[] { new Planet(), null, new Distance() };
            yield return new object[] { new Planet(), new Planet(), null };
        }

        public static IEnumerable<object[]> GetTestNullEmptyAircraftValues()
        {
            yield return new object[] { null };
            yield return new object[] { new EmpireAircraft() };
            yield return new object[] { new EmpireAircraft { Aircrafts = new List<EmpireAircraft.Aircraft>() } };
            yield return new object[] { new EmpireAircraft { Aircrafts = new List<EmpireAircraft.Aircraft>() } };
            yield return new object[] { new EmpireAircraft
            {
                Aircrafts = new List<EmpireAircraft.Aircraft>(),
                AircraftsTypes = new Dictionary<string, EmpireAircraft.AircraftType>()
            }};
        }

        #endregion

        #region Mocking

        private IAircraftFinderService Build()
        {
            return service;
        }

        /// <summary>
        /// Setups mock <see cref="webServiceMock"/> to return the given values <paramref name="aircraft"/>.
        /// </summary>
        /// <param name="aircraft">The aircraft values to mock.</param>
        /// <returns>The class itself</returns>
        private AircraftFinderServiceTest SetupWebServiceMockToReturnTheGivenValues(EmpireAircraft aircraft)
        {
            webServiceMock
                .Setup(m => m.GetAircrafts())
                .Returns(aircraft);
            return this;
        }
        
        /// <summary>
        /// Setups mock <see cref="webServiceMock"/> to return aircrafts.
        /// </summary>
        /// <returns>The class itself</returns>
        private AircraftFinderServiceTest SetupWebServiceMockToReturnAircrafts()
        {
            EmpireAircraft aircraft = new EmpireAircraft
            {
                Aircrafts = new List<EmpireAircraft.Aircraft>
                {
                    new EmpireAircraft.Aircraft
                    {
                        Reference = new Guid("8601b569-e9d6-440f-b160-4b9e1d030088"),
                        Sector = "1A",
                        Type = "cruise"
                    },
                    new EmpireAircraft.Aircraft
                    {
                        Reference = new Guid("bf88cb5f-9aa0-4cea-b9f4-b05f58b6fce5"),
                        Sector = "1A",
                        Type = "gunboat"
                    },
                    new EmpireAircraft.Aircraft
                    {
                        Reference = new Guid("b9808d37-8723-4591-8482-3c835a7408ae"),
                        Sector = "2A",
                        Type = "lightCruiser"
                    }
                },
                AircraftsTypes = new Dictionary<string, EmpireAircraft.AircraftType>
                {
                    { "cruise", new EmpireAircraft.AircraftType { MaxDistance = 100, SupportedAttack = 90, Crew = 100 }},
                    { "gunboat", new EmpireAircraft.AircraftType { MaxDistance = 60, SupportedAttack = 50, Crew = 200 }},
                    { "lightCruiser", new EmpireAircraft.AircraftType { MaxDistance = 50, SupportedAttack = 50, Crew = 10 }},
                }
            };

            webServiceMock
                .Setup(m => m.GetAircrafts())
                .Returns(aircraft);
            return this;
        }
        
        /// <summary>
        /// Setups mock <see cref="routeServiceMock"/> to return aircrafts.
        /// </summary>
        /// <returns>The class itself</returns>
        private AircraftFinderServiceTest SetupRouteServiceMockToReturnTotalRebelInfluence(Planet origin, Planet destination)
        {
            int total = origin.RebelInfluence + destination.RebelInfluence;

            routeServiceMock
                .Setup(m => m.CalculateRebelInfluence(origin, destination))
                .Returns(total);
            return this;
        }

        #endregion
    }
}
