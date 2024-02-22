using Serilog.Core;
using Serilog;
using System.Net;
using System.Net.Http;
using System.Web.Http.Filters;

namespace Imperial.CommercialRoutes.Application.Exceptions
{
    /// <summary>
    /// Represents the exception filter.
    /// </summary>
    public class GlobalExceptionFilter : ExceptionFilterAttribute
    {
        /// <summary>
        /// Raises the exception event.
        /// </summary>
        /// <param name="actionExecutedContext">The context for the action.</param>
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            string exceptionMessage = "Oops! Sorry! Something went wrong. Please contact your administrator.";
            string reasonPhase = "Internal Server Error";
            HttpStatusCode statusCode = HttpStatusCode.InternalServerError;

            switch (actionExecutedContext.Exception)
            {
                case BadRequestException badRequestException:
                    statusCode = HttpStatusCode.BadRequest;
                    reasonPhase = "Bad Request";
                    break;
                default:
                    break;
            }

            if (actionExecutedContext.Exception.InnerException == null)
            {
                exceptionMessage = actionExecutedContext.Exception.Message;
            }
            else
            {
                exceptionMessage = actionExecutedContext.Exception.InnerException.Message;
            }

            Logger logger = new LoggerConfiguration().ReadFrom.AppSettings().CreateLogger();
            logger.Error(actionExecutedContext.Exception, exceptionMessage);

            var response = new HttpResponseMessage(statusCode)
            {
                Content = new StringContent(exceptionMessage),
                ReasonPhrase = reasonPhase
            };
            actionExecutedContext.Response = response;
        }
    }
}
