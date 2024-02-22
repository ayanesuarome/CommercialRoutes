using AutoMapper;
using Imperial.CommercialRoutes.Application.DTOs;
using Imperial.CommercialRoutes.Application.Exceptions;
using Imperial.CommercialRoutes.Application.Interfaces.Services;
using Imperial.CommercialRoutes.Application.Interfaces.WebServices;
using Imperial.CommercialRoutes.Domain.Entities;
using Imperial.CommercialRoutes.Domain.Entities.Models;
using Imperial.CommercialRoutes.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Imperial.CommercialRoutes.Application.Services
{
    /// <summary>
    /// Service implementation for planet finder.
    /// </summary>
    public class PlanetFinderService : IPlanetFinderService
    {
        #region Fields

        private readonly IMapper _mapper;
        private readonly ISindicatePlanetWebService _planetWebService;
        private readonly IEmpireRebelInfluenceWebService _rebelInfluenceWebService;
        private readonly IPlanetRepository _planetRepository;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the class <see cref="PlanetFinderService"/>.
        /// </summary>
        /// <param name="mapper">Mapper instance.</param>
        /// <param name="planetWebService">Sindicate planet web service instance.</param>
        /// <param name="rebelInfluenceWebService">Empire spy report web service instance.</param>
        /// <param name="planetRepository">Planet repository instance.</param>
        public PlanetFinderService(
            IMapper mapper,
            ISindicatePlanetWebService planetWebService,
            IEmpireRebelInfluenceWebService rebelInfluenceWebService,
            IPlanetRepository planetRepository)
        {
            _mapper = mapper;
            _planetWebService = planetWebService;
            _rebelInfluenceWebService = rebelInfluenceWebService;
            _planetRepository = planetRepository;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets planets from internal data source. If there is no plant; searches for them from external source.
        /// </summary>
        /// <param name="options">Options to search specific planets.</param>
        /// <param name="cancellationToken">Used to propagate notifications that the operation should be canceled.</param>
        /// <returns>List of planets.</returns>
        public async Task<IList<Planet>> GetPlanetsAsync(
            PlanetSearchOptions options = null,
            CancellationToken cancellationToken = default)
        {
            IList<Planet> planets = await _planetRepository.GetPlanetsAsync(options, cancellationToken);

            if (planets == null || !planets.Any())
            {
                planets = new List<Planet>();
                IList<SindicatePlanet> sindicatePlanets = await _planetWebService.GetPlanetsAsync(options, cancellationToken);

                foreach (var (planet, rebelSearchOptions) in from SindicatePlanet sindicatePlanet in sindicatePlanets
                                                             let planet = _mapper.Map<Planet>(sindicatePlanet)
                                                             let rebelSearchOptions = new RebelSearchOptions
                                                             {
                                                                 Code = sindicatePlanet.Code
                                                             }
                                                             select (planet, rebelSearchOptions))
                {
                    Rebel rebelInfluence = (await _rebelInfluenceWebService.GetSpyReportAsync(rebelSearchOptions, cancellationToken))
                                        .FirstOrDefault()
                                        ??
                                        throw new Exception($"Could not find rebel influence for the planet with code '{rebelSearchOptions.Code}' in the spy report");

                    planet.RebelInfluence = rebelInfluence.RebelInfluence;
                    planets.Add(planet);
                }

                if(planets.Any())
                {
                    await _planetRepository.InsertAsync(planets);
                }
            }

            return planets;
        }

        /// <summary>
        /// Gets planet from internal data source. If there is no planet; searches from external source.
        /// </summary>
        /// <param name="options">Options to search specific planet.</param>
        /// <param name="cancellationToken">Used to propagate notifications that the operation should be canceled.</param>
        /// <returns>Planet.</returns>
        public async Task<Planet> GetPlanetAsync(
            PlanetSearchOptions options = null,
            CancellationToken cancellationToken = default)
        {
            if (options?.Code == null && options?.Name == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            Planet planet = await _planetRepository.GetPlanetAsync(options, cancellationToken);

            if (planet == null)
            {
                IList<SindicatePlanet> sindicatePlanets = await _planetWebService.GetPlanetsAsync(options, cancellationToken);

                if(sindicatePlanets == null || !sindicatePlanets.Any())
                {
                    return null;
                }

                SindicatePlanet sindicatePlanet = sindicatePlanets.FirstOrDefault();
                planet = _mapper.Map<Planet>(sindicatePlanet);
                RebelSearchOptions rebelSearchOptions = new RebelSearchOptions
                {
                    Code = sindicatePlanet.Code
                };

                IList<Rebel> rebelInfluences = await _rebelInfluenceWebService.GetSpyReportAsync(rebelSearchOptions, cancellationToken);

                if (rebelInfluences == null || !rebelInfluences.Any())
                {
                    throw new Exception($"Could not find rebel influence for the planet '{sindicatePlanet.Name}' in the spy report");
                }

                Rebel rebel = rebelInfluences.FirstOrDefault();
                planet.RebelInfluence = rebel.RebelInfluence;
                await _planetRepository.InsertAsync(planet);
            }

            return planet;
        }

        #endregion
    }
}
