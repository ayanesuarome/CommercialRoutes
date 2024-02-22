using Imperial.CommercialRoutes.Application.DTOs;
using Imperial.CommercialRoutes.Domain.Entities;

namespace Imperial.CommercialRoutes.Application.Interfaces.Services
{
    /// <summary>
    /// Service definition for aircraft finder.
    /// </summary>
    public interface IAircraftFinderService
    {
        /// <summary>
        /// Gets optimal aircraft for a route.
        /// </summary>
        /// <returns>Aircraft.</returns>
        EmpireAircraft.Aircraft GetOptimalAircraft(
            Planet origin,
            Planet destination,
            Distance distance);
    }
}
