using Imperial.CommercialRoutes.WebApi.IoC;
using System.Web;
using System.Web.Http;
using System.Web.Routing;

namespace Imperial.CommercialRoutes.WebApi
{
    public class WebApiApplication : HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            var dependency = new DependencyPublisher();
            var configuration = GlobalConfiguration.Configuration;
            dependency.RegisterDependencies(configuration);
        }
    }
}
