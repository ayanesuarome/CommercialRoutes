using AutoMapper;

namespace Imperial.CommercialRoutes.Application.AutoMapper
{
    public static class AutoMapperConfiguration
    {
        public static MapperConfiguration RegisterMappings()
        {
            return new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new SindicateDistanceToRouteProfile());
                cfg.AddProfile(new RouteToCommercialRouteProfile());
                cfg.AddProfile(new SindicatePlanetToPlanetProfile());
                cfg.AddProfile(new AircraftToAircraftResponse());
            });
        }
    }
}
