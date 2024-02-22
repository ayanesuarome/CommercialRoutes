using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Imperial.CommercialRoutes.Application.DTOs;
using Imperial.CommercialRoutes.Application.Exceptions;
using Imperial.CommercialRoutes.Application.Interfaces.Services;
using Imperial.CommercialRoutes.Application.Interfaces.WebServices;
using Imperial.CommercialRoutes.Application.Services;
using Imperial.CommercialRoutes.Domain.Entities;
using Imperial.CommercialRoutes.Domain.Entities.Models;

namespace Imperial.CommercialRoutes.Application
{
    public class CommercialRoutesService : ICommercialRoutesService
    {
        #region Fields

        private string DecimalFormat => ConfigurationManager.AppSettings.Get("DecimalFormat");
        private readonly IMapper _mapper;
        private readonly ISindicatePlanetWebService _sindicatePlanetWebService;
        private readonly ISindicateDistanceWebService _sindicateDistanceWebService;
        private readonly IPlanetFinderService _planetFinderService;
        private readonly IDistanceFinderService _distanceFinderService;
        private readonly IAircraftFinderService _aircraftFinderService;
        private readonly IBreakDownRoutePriceService _breakDownRoutePriceService;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the class <see cref="CommercialRoutesService"/>.
        /// </summary>
        /// <param name="mapper">Mapper instance.</param>
        /// <param name="sindicatePlanetWebService">Sindicate planet web service instance.</param>
        /// <param name="sindicateDistanceWebService">Sindicate distance web service instance.</param>
        /// <param name="planetFinderService">Planet finder service instance.</param>
        /// <param name="distanceFinderService">Distance finder service instance.</param>
        /// <param name="aircraftFinderService">Aircraft finder service instance.</param>
        /// <param name="breakDownRoutePriceService">Route breakdown price service instance.</param>
        public CommercialRoutesService(
            IMapper mapper,
            ISindicatePlanetWebService sindicatePlanetWebService,
            ISindicateDistanceWebService sindicateDistanceWebService,
            IPlanetFinderService planetFinderService,
            IDistanceFinderService distanceFinderService,
            IAircraftFinderService aircraftFinderService,
            IBreakDownRoutePriceService breakDownRoutePriceService)
        {
            _mapper = mapper;
            _sindicatePlanetWebService = sindicatePlanetWebService;
            _sindicateDistanceWebService = sindicateDistanceWebService;
            _planetFinderService = planetFinderService;
            _distanceFinderService = distanceFinderService;
            _aircraftFinderService = aircraftFinderService;
            _breakDownRoutePriceService = breakDownRoutePriceService;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets commercial routes.
        /// </summary>
        /// <remarks>
        /// Getting all the commercial routes of the imperial fleet galaxy is not scalable at all regardeless the current data set is short,
        /// millions of planets should exist and loading/storing all of them at once is not scalable at all and can lead into memory issues.
        /// </remarks>
        /// <param name="request">List of routes to search for.</param>
        /// <param name="cancellationToken">Used to propagate notifications that the operation should be canceled.</param>
        /// <returns>List of commercial routes.</returns>
        public async Task<IList<CommercialRoute>> GetCommercialRoutesAsync(CancellationToken cancellationToken = default)
        {
            IList<Distance> distances = await _distanceFinderService.GetDistancesAsync(cancellationToken: cancellationToken);
            IList<Planet> planets = await _planetFinderService.GetPlanetsAsync(cancellationToken: cancellationToken);
            IList<CommercialRoute> commercialRoutes = new List<CommercialRoute>();

            if (distances != null || !distances.Any() || planets != null || !planets.Any())
            {
                commercialRoutes = _mapper.Map<IList<CommercialRoute>>(distances);
            }

            foreach (CommercialRoute commercialRoute in commercialRoutes)
            {
                commercialRoute.Origin = planets.FirstOrDefault(p => p.Code == commercialRoute.Origin)?.Name;
                commercialRoute.Destination = planets.FirstOrDefault(p => p.Code == commercialRoute.Destination)?.Name;
                commercialRoute.Distance = Convert.ToDecimal(commercialRoute.Distance.ToString(DecimalFormat));
            }

            return commercialRoutes;
        }

        /// <summary>
        /// Gets price breakdown of a given distance.
        /// </summary>
        /// <param name="request">Route request.</param>
        /// <param name="cancellationToken">Used to propagate notifications that the operation should be canceled.</param>
        /// <returns>Price breakdown of the distance.</returns>
        public async Task<BreakdownRoutePrice> GetRoutePriceBreakdown(
            RouteRequest request,
            CancellationToken cancellationToken = default)
        {
            Planet origin = await GetPlanetByName(request.Origin, cancellationToken);
            Planet destination = await GetPlanetByName(request.Destination, cancellationToken);
            Distance distance = await GetDistanceByOriginAndDestination(origin.Code, destination.Code, cancellationToken);

            return await _breakDownRoutePriceService
                .CalculateBreakdownRoutePrice(request.DayOfWeek.Value,
                                        origin,
                                        destination,
                                        distance,
                                        cancellationToken);
        }

        /// <summary>
        /// Gets optimal aircraft.
        /// </summary>
        /// <param name="request">Route request.</param>
        /// <param name="cancellationToken">Used to propagate notifications that the operation should be canceled.</param>
        /// <returns></returns>
        public async Task<AircraftResponse> GetOptimalAircraft(
            RouteRequest request,
            CancellationToken cancellationToken = default)
        {
            Planet origin = await GetPlanetByName(request.Origin, cancellationToken);
            Planet destination = await GetPlanetByName(request.Destination, cancellationToken);
            Distance distance = await GetDistanceByOriginAndDestination(origin.Code, destination.Code, cancellationToken);

            EmpireAircraft.Aircraft aircraft = _aircraftFinderService.GetOptimalAircraft(
                origin,
                destination,
                distance);
            
            if (aircraft == null)
            {
                new AircraftResponse
                {
                    Message = $"Aircraft not found for the given route '<{request.Origin}-{request.Destination}>'"
                };
            }

            AircraftResponse response = _mapper.Map<AircraftResponse>(aircraft);
            response.Message = $"Optimal aircraft '{response.Type}' found for the given route '<{request.Origin}-{request.Destination}>'";

            return response;
        }

        #endregion

        #region Private Methods

        private async Task<Planet> GetPlanetByName(string name, CancellationToken cancellationToken = default)
        {
            Planet planet = await _planetFinderService.GetPlanetAsync(
                new PlanetSearchOptions
                {
                    Name = name
                    
                },
                cancellationToken);

            return planet ?? throw new BadRequestException($"Planet '{name}' not found.");
        }
        
        private async Task<Distance> GetDistanceByOriginAndDestination(string originCode, string destinationCode, CancellationToken cancellationToken = default)
        {
            Distance distance = await _distanceFinderService.GetDistanceAsync(originCode, destinationCode, cancellationToken);        
            return distance ?? throw new BadRequestException($"Destination '<{originCode}-{destinationCode}>' not found.");
        }

        #endregion
    }
}
