using AutoMapper;
using Imperial.CommercialRoutes.Application.DTOs;
using Imperial.CommercialRoutes.Domain.Entities;

namespace Imperial.CommercialRoutes.Application.AutoMapper
{
    /// <summary>
    /// Automapper profile.
    /// </summary>
    public class SindicatePlanetToPlanetProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the class <see cref="SindicatePlanetToPlanetProfile"/>.
        /// </summary>
        public SindicatePlanetToPlanetProfile()
        {
            CreateMap<SindicatePlanet, Planet>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.RebelInfluence, opt => opt.Ignore());
        }
    }
}
