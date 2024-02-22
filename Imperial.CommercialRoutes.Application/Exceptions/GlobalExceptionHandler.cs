using Serilog;
using Serilog.Core;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.ExceptionHandling;
using System.Web.Http.Results;

namespace Imperial.CommercialRoutes.Application.Exceptions
{
    public class GlobalExceptionHandler : ExceptionHandler
    {
        /// <summary>
        /// Handles the exception.
        /// </summary>
        /// <param name="context">The exception handler context.</param>
        /// <param name="cancellationToken">Used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A task representing the asynchronous exception handling operation.</returns>
        public async override Task HandleAsync(ExceptionHandlerContext context, CancellationToken cancellationToken)
        {
            Logger logger = new LoggerConfiguration().ReadFrom.AppSettings().CreateLogger();
            string errorMessage = "Oops! Sorry! Something went wrong. Please contact your administrator.";
            HttpStatusCode statusCode = HttpStatusCode.InternalServerError;

            logger.Fatal(context.Exception, errorMessage);

            var response = context.Request.CreateResponse(
                statusCode,
                new
                {
                    Message = errorMessage
                });
            response.Headers.Add("X-Error", errorMessage);
            context.Result = new ResponseMessageResult(response);
        }
    }
}
