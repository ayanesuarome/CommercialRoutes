using AutoMapper;
using Imperial.CommercialRoutes.Application.DTOs;
using Imperial.CommercialRoutes.Domain.Entities;

namespace Imperial.CommercialRoutes.Application.AutoMapper
{
    /// <summary>
    /// Automapper profile.
    /// </summary>
    public class RouteToCommercialRouteProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the class <see cref="RouteToCommercialRouteProfile"/>.
        /// </summary>
        public RouteToCommercialRouteProfile()
        {
            CreateMap<Distance, CommercialRoute>()
                .ForMember(dest => dest.Distance, opt => opt.MapFrom(s => s.LunarDays));
        }
    }
}
