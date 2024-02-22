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
    public class PlanetFinderServiceTest : IDisposable
    {
        #region Fields

        private PlanetFinderService service;
        private Mock<IMapper> mapperMock;
        private Mock<ISindicatePlanetWebService> planetWebServiceMock;
        private Mock<IEmpireRebelInfluenceWebService> rebelWebServiceMock;
        private Mock<IPlanetRepository> repositoryMock;

        #endregion

        #region Setup and Cleanup

        /// <summary>
        /// Initializes a new instance of the class <see cref="PlanetFinderServiceTest"/>.
        /// </summary>
        public PlanetFinderServiceTest()
        {
            mapperMock = new Mock<IMapper>();
            planetWebServiceMock = new Mock<ISindicatePlanetWebService>();
            rebelWebServiceMock = new Mock<IEmpireRebelInfluenceWebService>();
            repositoryMock = new Mock<IPlanetRepository>();
            service = new PlanetFinderService(
                mapperMock.Object,
                planetWebServiceMock.Object,
                rebelWebServiceMock.Object,
                repositoryMock.Object);
        }

        /// <summary>
        /// Releases unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            mapperMock = null;
            planetWebServiceMock = null;
            rebelWebServiceMock = null;
            repositoryMock = null;
            service = null;
        }

        #endregion

        #region Tests Methods

        /// <summary>
        /// Test that <see cref="IPlanetFinderService.GetPlanetAsync(PlanetSearchOptions, CancellationToken)"/>
        /// throws <see cref="ArgumentNullException"/> for null inputs.
        /// </summary>
        [Theory, MemberData(nameof(GetTestNullArguments))]
        public async Task TaskGetPlanetThrowsArgumentNullExceptionForNullInputs(PlanetSearchOptions options)
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => service.GetPlanetAsync(options, default));
        }

        /// <summary>
        /// Test that <see cref="IPlanetFinderService.GetPlanetAsync(PlanetSearchOptions, CancellationToken)"/>
        /// retuns null when the repository and web service returns null.
        /// </summary>
        [Fact]
        public async Task TaskGetPlanetReturnNull()
        {
            PlanetSearchOptions options = new PlanetSearchOptions
            {
                Code = "code",
            };
            IPlanetFinderService service = SetupRepositoryMockToReturnNullPlanet()
                .SetupPlanetWebServiceMockToReturnNull()
                .Build();

            Planet planet = await service.GetPlanetAsync(options, default);

            repositoryMock.Verify(m => m.GetPlanetAsync(It.IsAny<PlanetSearchOptions>(), default), Times.Once);
            planetWebServiceMock.Verify(m => m.GetPlanetsAsync(It.IsAny<PlanetSearchOptions>(), default), Times.Once);
            planet.Should().BeNull();
        }

        /// <summary>
        /// Test that <see cref="IPlanetFinderService.GetPlanetAsync(PlanetSearchOptions, CancellationToken)"/>
        /// retuns null when the repository returns null and web service an empty list.
        /// </summary>
        [Fact]
        public async Task TaskGetPlanetReturnNullForEmptyListOfPlanets()
        {
            PlanetSearchOptions options = new PlanetSearchOptions
            {
                Code = "code",
            };
            IPlanetFinderService service = SetupRepositoryMockToReturnNullPlanet()
                .SetupPlanetWebServiceMockToReturnEmptyList()
                .Build();

            Planet planet = await service.GetPlanetAsync(options, default);

            repositoryMock.Verify(m => m.GetPlanetAsync(It.IsAny<PlanetSearchOptions>(), default), Times.Once);
            planetWebServiceMock.Verify(m => m.GetPlanetsAsync(It.IsAny<PlanetSearchOptions>(), default), Times.Once);
            planet.Should().BeNull();
        }

        /// <summary>
        /// Test that <see cref="IPlanetFinderService.GetPlanetAsync(PlanetSearchOptions, CancellationToken)"/>
        /// retuns planet from the repository and not from the web service.
        /// </summary>
        [Theory, AutoData]
        public async Task TaskGetPlanetReturnPlanetFromRepository(Planet planetResult)
        {
            PlanetSearchOptions options = new PlanetSearchOptions
            {
                Code = "code",
            };
            IPlanetFinderService service = SetupRepositoryMockToReturnPlanet(planetResult)
                .Build();

            Planet planet = await service.GetPlanetAsync(options, default);

            repositoryMock.Verify(m => m.GetPlanetAsync(It.IsAny<PlanetSearchOptions>(), default), Times.Once);
            planetWebServiceMock.Verify(m => m.GetPlanetsAsync(It.IsAny<PlanetSearchOptions>(), default), Times.Never);
            repositoryMock.Verify(m => m.InsertAsync(It.IsAny<Planet>(), default), Times.Never);
            planet.Should().NotBeNull().And.Be(planet);
        }

        /// <summary>
        /// Test that <see cref="IPlanetFinderService.GetPlanetAsync(PlanetSearchOptions, CancellationToken)"/>
        /// retuns planet from the sindicate web service.
        /// </summary>
        [Fact]
        public async Task TaskGetPlanetReturnPlanetFromFromWebService()
        {
            PlanetSearchOptions options = new PlanetSearchOptions
            {
                Code = "code",
            };
            IPlanetFinderService service = SetupRepositoryMockToReturnNullPlanet()
                .SetupPlanetWebServiceMockToReturnPlanet()
                .SetupMapperMockToMapSindicatePlanetToPlanet()
                .SetupRebelWebServiceMockToReturnRebels()
                .Build();

            Planet planet = await service.GetPlanetAsync(options, default);

            repositoryMock.Verify(m => m.GetPlanetAsync(options, default), Times.Once);
            planetWebServiceMock.Verify(m => m.GetPlanetsAsync(options, default), Times.Once);
            rebelWebServiceMock.Verify(m => m.GetSpyReportAsync(It.IsAny<RebelSearchOptions>(), default), Times.Once);
            mapperMock.Verify(m => m.Map<Planet>(It.IsAny<SindicatePlanet>()), Times.Once);
            repositoryMock.Verify(m => m.InsertAsync(It.IsAny<Planet>(), default), Times.Once);

            planet.Should().NotBeNull();
            planet.RebelInfluence.Should().Be(10);
        }

        /// <summary>
        /// Test that <see cref="IPlanetFinderService.GetPlanetAsync(PlanetSearchOptions, CancellationToken)"/>
        /// throws <see cref="Exception"/> for null rebel influence report.
        /// </summary>
        [Fact]
        public async Task TaskGetPlanetThrowExceptionForNullRebelInfluence()
        {
            PlanetSearchOptions options = new PlanetSearchOptions
            {
                Code = "code",
            };
            IPlanetFinderService service = SetupRepositoryMockToReturnNullPlanet()
                .SetupPlanetWebServiceMockToReturnPlanet()
                .SetupMapperMockToMapSindicatePlanetToPlanet()
                .SetupRebelWebServiceMockToReturnNull()
                .Build();

            await Assert.ThrowsAsync<Exception>(() => service.GetPlanetAsync(options, default));

            repositoryMock.Verify(m => m.GetPlanetAsync(options, default), Times.Once);
            planetWebServiceMock.Verify(m => m.GetPlanetsAsync(options, default), Times.Once);
            rebelWebServiceMock.Verify(m => m.GetSpyReportAsync(It.IsAny<RebelSearchOptions>(), default), Times.Once);
            mapperMock.Verify(m => m.Map<Planet>(It.IsAny<SindicatePlanet>()), Times.Once);
            repositoryMock.Verify(m => m.InsertAsync(It.IsAny<Planet>(), default), Times.Never);
        }

        #endregion

        #region Member Test Data

        public static IEnumerable<object[]> GetTestNullArguments()
        {
            yield return new object[] { null };
            yield return new object[] { new PlanetSearchOptions() };
        }

        #endregion

        #region Mocking

        private IPlanetFinderService Build()
        {
            return service;
        }

        /// <summary>
        /// Setups mock <see cref="repositoryMock"/> to return null planet.
        /// </summary>
        /// <returns>The class itself</returns>
        private PlanetFinderServiceTest SetupRepositoryMockToReturnNullPlanet()
        {
            repositoryMock
                .Setup(m => m.GetPlanetAsync(It.IsAny<PlanetSearchOptions>(), default))
                .ReturnsAsync(() => null);
            return this;
        }

        /// <summary>
        /// Setups mock <see cref="repositoryMock"/> to return a planet.
        /// </summary>
        /// <param name="planet">Planet to resturn.</param>
        /// <returns>The class itself</returns>
        private PlanetFinderServiceTest SetupRepositoryMockToReturnPlanet(Planet planet)
        {
            repositoryMock
                .Setup(m => m.GetPlanetAsync(It.IsAny<PlanetSearchOptions>(), default))
                .ReturnsAsync(planet);
            return this;
        }

        /// <summary>
        /// Setups mock <see cref="planetWebServiceMock"/> to return null.
        /// </summary>
        /// <returns>The class itself</returns>
        private PlanetFinderServiceTest SetupPlanetWebServiceMockToReturnNull()
        {
            planetWebServiceMock
                .Setup(m => m.GetPlanetsAsync(It.IsAny<PlanetSearchOptions>(), default))
                .ReturnsAsync(() => null);
            return this;
        }
        
        /// <summary>
        /// Setups mock <see cref="planetWebServiceMock"/> to return empty list.
        /// </summary>
        /// <returns>The class itself</returns>
        private PlanetFinderServiceTest SetupPlanetWebServiceMockToReturnEmptyList()
        {
            planetWebServiceMock
                .Setup(m => m.GetPlanetsAsync(It.IsAny<PlanetSearchOptions>(), default))
                .ReturnsAsync(new List<SindicatePlanet>());
            return this;
        }

        /// <summary>
        /// Setups mock <see cref="planetWebServiceMock"/> to return a planet.
        /// </summary>
        /// <returns>The class itself</returns>
        private PlanetFinderServiceTest SetupPlanetWebServiceMockToReturnPlanet()
        {
            IList<SindicatePlanet> planets = new List<SindicatePlanet>
            {
                new SindicatePlanet
                {
                    Code = "code",
                    Name = "name",
                    Sector = "1A"
                }
            };

            planetWebServiceMock
                .Setup(m => m.GetPlanetsAsync(It.IsAny<PlanetSearchOptions>(), default))
                .ReturnsAsync(planets);
            return this;
        }

        /// <summary>
        /// Setups mock <see cref="rebelWebServiceMock"/> to return null.
        /// </summary>
        /// <returns>The class itself</returns>
        private PlanetFinderServiceTest SetupRebelWebServiceMockToReturnNull()
        {
            rebelWebServiceMock
                .Setup(m => m.GetSpyReportAsync(It.IsAny<RebelSearchOptions>(), default))
                .ReturnsAsync(() => null);
            return this;
        }

        /// <summary>
        /// Setups mock <see cref="rebelWebServiceMock"/> to return a rebel.
        /// </summary>
        /// <returns>The class itself</returns>
        private PlanetFinderServiceTest SetupRebelWebServiceMockToReturnRebels()
        {
            IList<Rebel> rebels = new List<Rebel>
            {
                new Rebel
                {
                    Code = "code",
                    RebelInfluence = 10
                }
            };

            rebelWebServiceMock
                .Setup(m => m.GetSpyReportAsync(It.IsAny<RebelSearchOptions>(), default))
                .ReturnsAsync(rebels);
            return this;
        }

        /// <summary>
        /// Setups mock <see cref="mapperMock"/> to map from <see cref="SindicatePlanet"/> to <see cref="Planet"/>.
        /// </summary>
        /// <returns>The class itself</returns>
        private PlanetFinderServiceTest SetupMapperMockToMapSindicatePlanetToPlanet()
        {
            Planet planet = new Planet
            {
                Code = "code",
                Name = "name",
                Sector = "1A",
            };

            mapperMock
                .Setup(m => m.Map<Planet>(It.IsAny<SindicatePlanet>()))
                .Returns(planet);
            return this;
        }

        #endregion
    }
}
