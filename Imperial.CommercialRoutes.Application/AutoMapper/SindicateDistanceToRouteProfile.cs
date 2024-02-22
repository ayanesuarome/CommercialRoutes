using AutoMapper;
using Imperial.CommercialRoutes.Application.DTOs;
using Imperial.CommercialRoutes.Domain.Entities;

namespace Imperial.CommercialRoutes.Application.AutoMapper
{
    /// <summary>
    /// Automapper profile.
    /// </summary>
    public class SindicateDistanceToRouteProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the class <see cref="SindicateDistanceToRouteProfile"/>.
        /// </summary>
        public SindicateDistanceToRouteProfile()
        {
            CreateMap<SindicateDistance, Distance>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Origin, opt => opt.Ignore())
                .ForMember(dest => dest.Destination, opt => opt.MapFrom(s => s.Code));
        }
    }
}
