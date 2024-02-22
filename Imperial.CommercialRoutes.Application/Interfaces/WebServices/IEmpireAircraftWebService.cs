using Imperial.CommercialRoutes.Application.DTOs;

namespace Imperial.CommercialRoutes.Application.Interfaces.WebServices
{
    /// <summary>
    /// Service definition for empire aircraft.
    /// </summary>
    public interface IEmpireAircraftWebService
    {
        /// <summary>
        /// Gets aircrafts.
        /// </summary>
        /// <returns>Aircrafts.</returns>
        EmpireAircraft GetAircrafts();
    }
}
