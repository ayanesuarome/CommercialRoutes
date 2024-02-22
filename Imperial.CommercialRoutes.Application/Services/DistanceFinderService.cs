using AutoMapper;
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
    /// Service implementation for distance finder.
    /// </summary>
    public class DistanceFinderService : IDistanceFinderService
    {
        #region Fields

        private readonly IMapper _mapper;
        private readonly ISindicateDistanceWebService _distanceWebService;
        private readonly IDistanceRepository _distanceRepository;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the class <see cref="DistanceFinderService"/>.
        /// </summary>
        /// <param name="mapper">Mapper instance.</param>
        /// <param name="distanceWebService">Sindicate distance web service instance.</param>
        /// <param name="distanceRepository">Distance repository instance.</param>
        public DistanceFinderService(
            IMapper mapper,
            ISindicateDistanceWebService distanceWebService,
            IDistanceRepository distanceRepository)
        {
            _mapper = mapper;
            _distanceWebService = distanceWebService;
            _distanceRepository = distanceRepository;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets list of distances from internal data source. If there is no distance; searches from external source.
        /// </summary>
        /// <param name="options">Options to search specific distances.</param>
        /// <param name="cancellationToken">Used to propagate notifications that the operation should be canceled.</param>
        /// <returns>List of distances.</returns>
        public async Task<IList<Distance>> GetDistancesAsync(
            DistanceSearchOptions options = null,
            CancellationToken cancellationToken = default)
        {
            IList<Distance> distances = await _distanceRepository.GetDistancesAsync(options, cancellationToken);

            if (distances == null || !distances.Any())
            {
                distances = new List<Distance>();
                var sindicateDistances = await _distanceWebService.GetDistancesAsync(
                    options?.Origin,
                    options?.Destination,
                    cancellationToken);
                
                foreach (var (key, distance) in from key in sindicateDistances.Keys
                                             let mappedDistances = _mapper.Map<List<Distance>>(sindicateDistances[key])
                                             from distance in mappedDistances
                                                select (key, distance))
                {
                    distance.Origin = key;
                    distances.Add(distance);
                }

                if(distances.Any())
                {
                    await _distanceRepository.InsertAsync(distances);
                }
            }

            return distances;
        }

        /// <summary>
        /// Gets distance from internal data source. If there is no distance; searches from external source.
        /// </summary>
        /// <param name="origin">Origin of the distance to search for.</param>
        /// <param name="destination">Destination of the distance to search for.</param>
        /// <param name="cancellationToken">Used to propagate notifications that the operation should be canceled.</param>
        /// <returns>Distances.</returns>
        public async Task<Distance> GetDistanceAsync(
            string origin,
            string destination,
            CancellationToken cancellationToken = default)
        {
            if(String.IsNullOrWhiteSpace(origin))
            {
                throw new ArgumentNullException(nameof(origin));
            }
            if (String.IsNullOrWhiteSpace(destination))
            {
                throw new ArgumentNullException(nameof(destination));
            }

            DistanceSearchOptions options = new DistanceSearchOptions
            {
                Origin = origin,
                Destination = destination
            };
            Distance distance = await _distanceRepository.GetDistanceAsync(options, cancellationToken);

            if (distance == null)
            {
                var sindicateDistance = (await _distanceWebService.GetDistancesAsync(
                    options.Origin,
                    options.Destination,
                    cancellationToken))
                    .FirstOrDefault();

                if (sindicateDistance.Key == null || !sindicateDistance.Value.Any())
                {
                    return null;
                }

                var destinationToMap = sindicateDistance.Value.FirstOrDefault(d => d.Code == options.Destination);
                distance = _mapper.Map<Distance>(destinationToMap);
                distance.Origin = sindicateDistance.Key;

                if (distance != null)
                {
                    await _distanceRepository.InsertAsync(distance);
                }
            }

            return distance;
        }

        #endregion
    }
}
