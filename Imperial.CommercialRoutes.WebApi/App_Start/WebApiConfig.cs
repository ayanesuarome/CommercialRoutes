using FluentValidation.WebApi;
using Imperial.CommercialRoutes.Application.Exceptions;
using Imperial.CommercialRoutes.WebApi.App_Start;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;

namespace Imperial.CommercialRoutes.WebApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            config.Services.Replace(typeof(IExceptionHandler), new GlobalExceptionHandler());
            
            // Add Custom validation filters  
            config.Filters.Add(new ValidateModelStateFilter());
            FluentValidationModelValidatorProvider.Configure(config);
            config.Filters.Add(new GlobalExceptionFilter());

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
