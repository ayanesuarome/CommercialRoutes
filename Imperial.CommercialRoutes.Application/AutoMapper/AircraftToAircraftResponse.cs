using AutoMapper;
using Imperial.CommercialRoutes.Application.DTOs;

namespace Imperial.CommercialRoutes.Application.AutoMapper
{
    /// <summary>
    /// Automapper profile.
    /// </summary>
    public class AircraftToAircraftResponse : Profile
    {
        /// <summary>
        /// Initializes a new instance of the class <see cref="AircraftToAircraftResponse"/>.
        /// </summary>
        public AircraftToAircraftResponse()
        {
            CreateMap<EmpireAircraft.Aircraft, AircraftResponse>()
                .ForMember(dest => dest.Message, opt => opt.Ignore());
        }
    }
}
