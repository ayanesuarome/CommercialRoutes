using AutoFixture.Xunit2;
using AutoMapper;
using FluentAssertions;
using Imperial.CommercialRoutes.Application.DTOs;
using Imperial.CommercialRoutes.Application.Interfaces.Services;
using Imperial.CommercialRoutes.Application.Interfaces.WebServices;
using Imperial.CommercialRoutes.Application.Services;
using Imperial.CommercialRoutes.Domain.Entities;
using Imperial.CommercialRoutes.Domain.Entities.Models;
using Imperial.CommercialRoutes.Domain.Interfaces;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Imperial.CommercialRoutes.Application.Tests.Services
{
    public class DistanceFinderServiceTest : IDisposable
    {
        #region Fields

        private DistanceFinderService service;
        private Mock<ISindicateDistanceWebService> webServiceMock;
        private Mock<IMapper> mapperMock;
        private Mock<IDistanceRepository> repositoryMock;

        #endregion

        #region Setup and Cleanup

        /// <summary>
        /// Initializes a new instance of the class <see cref="DistanceFinderServiceTest"/>.
        /// </summary>
        public DistanceFinderServiceTest()
        {
            mapperMock = new Mock<IMapper>();
            webServiceMock = new Mock<ISindicateDistanceWebService>();
            repositoryMock = new Mock<IDistanceRepository>();
            service = new DistanceFinderService(
                mapperMock.Object,
                webServiceMock.Object,
                repositoryMock.Object);
        }

        /// <summary>
        /// Releases unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            webServiceMock = null;
            mapperMock = null;
            repositoryMock = null;
            service = null;
        }

        #endregion

        #region Tests Methods

        /// <summary>
        /// Test that <see cref="IDistanceFinderService.GetDistanceAsync(string, string, CancellationToken)"/>
        /// throws <see cref="ArgumentNullException"/> for null inputs.
        /// </summary>
        [Theory]
        [InlineData(null, "destination")]
        [InlineData("origin", null)]
        public async Task TaskGetDistanceThrowsArgumentNullExceptionForNullInputs(string origin, string destination)
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => service.GetDistanceAsync(origin, destination));
        }

        /// <summary>
        /// Test that <see cref="IDistanceFinderService.GetDistanceAsync(string, string, CancellationToken)"/>
        /// retuns null when the repository and web service returns null and/or empty dictionary.
        /// </summary>
        [Fact]
        public async Task TaskGetDistanceReturnNull()
        {
            IDistanceFinderService service = SetupRepositoryMockToReturnNullDistance()
                .SetupWebServiceMockToReturnEmptyDictionary()
                .Build();

            Distance distance = await service.GetDistanceAsync("origin", "destination");

            repositoryMock.Verify(m => m.GetDistanceAsync(It.IsAny<DistanceSearchOptions>(), default), Times.Once);
            webServiceMock.Verify(m => m.GetDistancesAsync(It.IsAny<string>(), It.IsAny<string>(), default), Times.Once);
            distance.Should().BeNull();
        }

        /// <summary>
        /// Test that <see cref="IDistanceFinderService.GetDistanceAsync(string, string, CancellationToken)"/>
        /// retuns distance from the repository and not from the web service.
        /// </summary>
        [Theory, AutoData]
        public async Task TaskGetDistanceReturnDistanceFromRepository(Distance distanceResult)
        {
            IDistanceFinderService service = SetupRepositoryMockToReturnDistance(distanceResult)
                .Build();

            Distance distance = await service.GetDistanceAsync("origin", "destination");

            repositoryMock.Verify(m => m.GetDistanceAsync(It.IsAny<DistanceSearchOptions>(), default), Times.Once);
            webServiceMock.Verify(m => m.GetDistancesAsync(It.IsAny<string>(), It.IsAny<string>(), default), Times.Never);
            repositoryMock.Verify(m => m.InsertAsync(It.IsAny<Distance>(), default), Times.Never);
            distance.Should().NotBeNull().And.Be(distanceResult);
        }
        
        /// <summary>
        /// Test that <see cref="IDistanceFinderService.GetDistanceAsync(string, string, CancellationToken)"/>
        /// retuns distance from the sindicate web service.
        /// </summary>
        [Fact]
        public async Task TaskGetDistanceReturnDistanceFromWebService()
        {
            IDistanceFinderService service = SetupRepositoryMockToReturnNullDistance()
                .SetupWebServiceMockToReturnDistance("origin", "destination")
                .SetupMapperMockToMapSindicateDistanceToDistance("origin", "destination", 5)
                .Build();

            Distance distance = await service.GetDistanceAsync("origin", "destination");

            repositoryMock.Verify(m => m.GetDistanceAsync(It.IsAny<DistanceSearchOptions>(), default), Times.Once);
            webServiceMock.Verify(m => m.GetDistancesAsync("origin", "destination", default), Times.Once);
            mapperMock.Verify(m => m.Map<Distance>(It.IsAny<SindicateDistance>()), Times.Once);
            repositoryMock.Verify(m => m.InsertAsync(It.IsAny<Distance>(), default), Times.Once);
            distance.Should().NotBeNull();
            distance.Origin.Should().NotBeNull().And.Be("origin");
            distance.Destination.Should().NotBeNull().And.Be("destination");
            distance.LunarYears.Should().Be(5);
            distance.LunarDays.Should().Be(distance.LunarYears * 365);
        }
        
        /// <summary>
        /// Test that <see cref="IDistanceFinderService.GetDistancesAsync(DistanceSearchOptions, CancellationToken)"/>
        /// retuns null when the repository and web service returns null and/or empty dictionary.
        /// </summary>
        [Fact]
        public async Task TaskGetDistancesReturnNullForNullDistances()
        {
            var options = new DistanceSearchOptions
            {
                Origin = "origin",
                Destination = "destination"
            };
            IDistanceFinderService service = SetupRepositoryMockToReturnNullDistances()
                .SetupWebServiceMockToReturnEmptyDictionary()
                .Build();

            IList<Distance> distances = await service.GetDistancesAsync(options);

            repositoryMock.Verify(m => m.GetDistancesAsync(options, default), Times.Once);
            webServiceMock.Verify(m => m.GetDistancesAsync(options.Origin, options.Destination, default), Times.Once);
            repositoryMock.Verify(m => m.InsertAsync(It.IsAny<IList<Distance>>(), default), Times.Never);
            distances.Should().BeEmpty();
        }
        
        /// <summary>
        /// Test that <see cref="IDistanceFinderService.GetDistancesAsync(DistanceSearchOptions, CancellationToken)"/>
        /// retuns null when the repository and web service returns empty list and/or empty dictionary.
        /// </summary>
        [Fact]
        public async Task TaskGetDistancesReturnNullForEmptyDistances()
        {
            var options = new DistanceSearchOptions
            {
                Origin = "origin",
                Destination = "destination"
            };
            IDistanceFinderService service = SetupRepositoryMockToReturnEmptyDistances()
                .SetupWebServiceMockToReturnEmptyDictionary()
                .Build();

            IList<Distance> distances = await service.GetDistancesAsync(options);

            repositoryMock.Verify(m => m.GetDistancesAsync(options, default), Times.Once);
            webServiceMock.Verify(m => m.GetDistancesAsync(options.Origin, options.Destination, default), Times.Once);
            repositoryMock.Verify(m => m.InsertAsync(It.IsAny<IList<Distance>>(), default), Times.Never);
            distances.Should().BeEmpty();
        }

        /// <summary>
        /// Test that <see cref="IDistanceFinderService.GetDistancesAsync(DistanceSearchOptions, CancellationToken)"/>
        /// retuns distances from the repository and not from the web service.
        /// </summary>
        [Theory, AutoData]
        public async Task TaskGetDistanceReturnDistancesFromRepository(IList<Distance> distancesResult)
        {
            var options = new DistanceSearchOptions
            {
                Origin = "origin",
                Destination = "destination"
            };
            IDistanceFinderService service = SetupRepositoryMockToReturnDistances(distancesResult)
                .Build();

            IList<Distance> distances = await service.GetDistancesAsync(options);

            repositoryMock.Verify(m => m.GetDistancesAsync(options, default), Times.Once);
            webServiceMock.Verify(m => m.GetDistancesAsync(options.Origin, options.Destination, default), Times.Never);
            repositoryMock.Verify(m => m.InsertAsync(It.IsAny<IList<Distance>>(), default), Times.Never);
            distances.Should().NotBeNull();
        }
        
        /// <summary>
        /// Test that <see cref="IDistanceFinderService.GetDistancesAsync(DistanceSearchOptions, CancellationToken)"/>
        /// retuns a list of distances from the sindicate web service.
        /// </summary>
        [Fact]
        public async Task TaskGetDistanceReturnDistancesFromWebService()
        {
            IDistanceFinderService service = SetupRepositoryMockToReturnNullDistance()
                .SetupWebServiceMockToReturnDistance("origin", "destination")
                .SetupMapperMockToMapSindicateDistanceToDistance("origin", "destination", 5)
                .Build();

            Distance distance = await service.GetDistanceAsync("origin", "destination");

            repositoryMock.Verify(m => m.GetDistanceAsync(It.IsAny<DistanceSearchOptions>(), default), Times.Once);
            webServiceMock.Verify(m => m.GetDistancesAsync("origin", "destination", default), Times.Once);
            mapperMock.Verify(m => m.Map<Distance>(It.IsAny<SindicateDistance>()), Times.Once);
            repositoryMock.Verify(m => m.InsertAsync(It.IsAny<Distance>(), default), Times.Once);
            distance.Should().NotBeNull();
            distance.Origin.Should().NotBeNull().And.Be("origin");
            distance.Destination.Should().NotBeNull().And.Be("destination");
            distance.LunarYears.Should().Be(5);
            distance.LunarDays.Should().Be(distance.LunarYears * 365);
        }

        #endregion

        #region Member Test Data

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

        private IDistanceFinderService Build()
        {
            return service;
        }

        /// <summary>
        /// Setups mock <see cref="repositoryMock"/> to return null distance.
        /// </summary>
        /// <returns>The class itself</returns>
        private DistanceFinderServiceTest SetupRepositoryMockToReturnNullDistance()
        {
            repositoryMock
                .Setup(m => m.GetDistanceAsync(It.IsAny<DistanceSearchOptions>(), default))
                .ReturnsAsync(() => null);
            return this;
        }

        /// <summary>
        /// Setups mock <see cref="repositoryMock"/> to return null distances.
        /// </summary>
        /// <returns>The class itself</returns>
        private DistanceFinderServiceTest SetupRepositoryMockToReturnNullDistances()
        {
            repositoryMock
                .Setup(m => m.GetDistancesAsync(It.IsAny<DistanceSearchOptions>(), default))
                .ReturnsAsync(() => null);
            return this;
        }

        /// <summary>
        /// Setups mock <see cref="repositoryMock"/> to return empty list of distances.
        /// </summary>
        /// <returns>The class itself</returns>
        private DistanceFinderServiceTest SetupRepositoryMockToReturnEmptyDistances()
        {
            repositoryMock
                .Setup(m => m.GetDistancesAsync(It.IsAny<DistanceSearchOptions>(), default))
                .ReturnsAsync(new List<Distance>());
            return this;
        }

        /// <summary>
        /// Setups mock <see cref="webServiceMock"/> to return null.
        /// </summary>
        /// <returns>The class itself</returns>
        private DistanceFinderServiceTest SetupWebServiceMockToReturnEmptyDictionary()
        {
            webServiceMock
                .Setup(m => m.GetDistancesAsync(It.IsAny<string>(), It.IsAny<string>(), default))
                .ReturnsAsync(new Dictionary<string, IEnumerable<SindicateDistance>>());
            return this;
        }

        /// <summary>
        /// Setups mock <see cref="repositoryMock"/> to return a distance.
        /// </summary>
        /// <param name="distance">Distance to resturn.</param>
        /// <returns>The class itself</returns>
        private DistanceFinderServiceTest SetupRepositoryMockToReturnDistance(Distance distance)
        {
            repositoryMock
                .Setup(m => m.GetDistanceAsync(It.IsAny<DistanceSearchOptions>(), default))
                .ReturnsAsync(distance);
            return this;
        }
        
        /// <summary>
        /// Setups mock <see cref="repositoryMock"/> to return a list of distances.
        /// </summary>
        /// <param name="distances">Distances to resturn.</param>
        /// <returns>The class itself</returns>
        private DistanceFinderServiceTest SetupRepositoryMockToReturnDistances(IList<Distance> distances)
        {
            repositoryMock
                .Setup(m => m.GetDistancesAsync(It.IsAny<DistanceSearchOptions>(), default))
                .ReturnsAsync(distances);
            return this;
        }

        /// <summary>
        /// Setups mock <see cref="webServiceMock"/> to return a distance.
        /// </summary>
        /// <param name="origin">Origin.</param>
        /// <param name="destination">Destination.</param>
        /// <returns>The class itself</returns>
        private DistanceFinderServiceTest SetupWebServiceMockToReturnDistance(string origin, string destination)
        {
            var distance = new Dictionary<string, IEnumerable<SindicateDistance>>
            {
                {
                    origin,
                    new List<SindicateDistance>
                {
                    new SindicateDistance
                    {
                        Code = destination,
                        LunarYears = 5
                    }
                }
                }
            };

            webServiceMock
                .Setup(m => m.GetDistancesAsync(origin, destination, default))
                .ReturnsAsync(distance);
            return this;
        }

        /// <summary>
        /// Setups mock <see cref="mapperMock"/> to map from <see cref="SindicateDistance"/> to <see cref="Distance"/>.
        /// </summary>
        /// <param name="origin">Origin.</param>
        /// <param name="destination">Destination.</param>
        /// <param name="lunarYears">Lunar years.</param>
        /// <returns>The class itself</returns>
        private DistanceFinderServiceTest SetupMapperMockToMapSindicateDistanceToDistance(string origin, string destination, int lunarYears)
        {
            Distance distance = new Distance
            {
                Destination = destination,
                LunarYears = lunarYears
            };

            mapperMock
                .Setup(m => m.Map<Distance>(It.IsAny<SindicateDistance>()))
                .Returns(distance);
            return this;
        }

        #endregion
    }
}
