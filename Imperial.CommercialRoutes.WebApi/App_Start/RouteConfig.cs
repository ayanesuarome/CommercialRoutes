using Swashbuckle.Application;
using System.Web.Http;
using System.Web.Routing;

namespace Imperial.CommercialRoutes.WebApi
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.MapHttpRoute(
                name: "swagger_root",
                routeTemplate: "",
                defaults: null,
                constraints: null,
                handler: new RedirectHandler((message => message.RequestUri.ToString()), "swagger")
            );
        }
    }
}