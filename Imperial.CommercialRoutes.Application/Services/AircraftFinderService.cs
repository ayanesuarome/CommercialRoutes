using Imperial.CommercialRoutes.Application.DTOs;
using Imperial.CommercialRoutes.Application.Interfaces.Services;
using Imperial.CommercialRoutes.Application.Interfaces.WebServices;
using Imperial.CommercialRoutes.Domain.Entities;
using Imperial.CommercialRoutes.Domain.Entities.Models;
using Imperial.CommercialRoutes.Domain.Interfaces;
using System;
using System.Linq;

namespace Imperial.CommercialRoutes.Application.Services
{
    /// <summary>
    /// Service implementation for aircraft finder.
    /// </summary>
    public class AircraftFinderService : IAircraftFinderService
    {
        #region Fields

        private readonly IEmpireAircraftWebService _webService;
        private readonly IRouteService _routeService;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the class <see cref="AircraftFinderService"/>.
        /// </summary>
        /// <param name="webService">Empire aircraft service instance.</param>
        /// <param name="routeService">Route domain service instance.</param>
        public AircraftFinderService(
            IEmpireAircraftWebService webService,
            IRouteService routeService)
        {
            _webService = webService;
            _routeService = routeService;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets optimal aircraft for a route.
        /// </summary>
        /// <returns>Aircraft.</returns>
        public EmpireAircraft.Aircraft GetOptimalAircraft(
            Planet origin,
            Planet destination,
            Distance distance)
        {
            if(origin == null)
            {
                throw new ArgumentNullException(nameof(origin));
            }
            if (destination == null)
            {
                throw new ArgumentNullException(nameof(destination));
            }
            if (distance == null)
            {
                throw new ArgumentNullException(nameof(distance));
            }

            EmpireAircraft aircraft = _webService.GetAircrafts();

            if (aircraft == null || aircraft.Aircrafts == null || aircraft.AircraftsTypes == null)
            {
                return null;
            }

            int totalRebelInfluence = _routeService.CalculateRebelInfluence(origin, destination);
            var valuePair = aircraft.AircraftsTypes.Where(pair => pair.Value.MaxDistance >= distance.LunarYears && pair.Value.SupportedAttack >= totalRebelInfluence)
                                                    .OrderBy(pair => pair.Value.Crew)
                                                    .FirstOrDefault();

            return aircraft.Aircrafts.FirstOrDefault(a => a.Type == valuePair.Key);
        }

        #endregion
    }
}
