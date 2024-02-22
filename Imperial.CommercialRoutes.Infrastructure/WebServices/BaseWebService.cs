using RestSharp;
using System;
using System.Threading;
using System.Threading.Tasks;
using Serilog;

namespace Imperial.CommercialRoutes.Infrastructure.WebServices
{
    /// <summary>
    /// Base class for all the HTTP client services.
    /// </summary>
    public class BaseWebService
    {
        #region Fields

        protected readonly ILogger Logger;
        protected readonly IRestClient Client;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the class <see cref="BaseWebService"/>.
        /// </summary>
        /// <param name="logger">Logger instance.</param>
        /// <param name="client">HTTP client instance.</param>
        public BaseWebService(
            ILogger logger,
            IRestClient client)
        {
            Logger = logger;
            Client = client;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the <see cref="RestRequest"/> instance with configurations to make requests.
        /// </summary>
        /// <param name="resource">Resource to use.</param>
        /// <param name="method">Method to use (defaults to Method.Get>.</param>
        /// <param name="jsonBody">Object to serialized as JSON.</param>
        /// <returns>The request configured with the provided values.</returns>
        protected virtual RestRequest GetRestRequest(string resource,
            Method method,
            object jsonBody = null)
        {
            var request = new RestRequest(resource, method);

            if (jsonBody != null)
            {
                request.AddJsonBody(jsonBody);
            }

            return request;
        }

        /// <summary>
        /// Gets the <see cref="RestRequest"/> instance with configurations to make requests asynchronously.
        /// </summary>
        /// <param name="resource">Resource to use.</param>
        /// <param name="method">Method to use (defaults to Method.Get>.</param>
        /// <param name="cancellationToken">Used to propagate notifications that the operation should be canceled.</param>
        /// <typeparam name="TResult">Response result type.</typeparam>
        /// <returns>The result.</returns>
        protected virtual async Task<TResult> ExecuteAsync<TResult>(
            RestRequest request,
            CancellationToken cancellationToken = default)
        {
            var response = await Client.ExecuteAsync<TResult>(request, cancellationToken);

            if (!response.IsSuccessful)
            {
                Logger.Warning("Could not fetch data. Fault response code: {0}.", response.StatusCode);
            }
            if (response.ErrorException != null)
            {
                string errorMessage = "Could not fetch data.";
                Logger.Error(response.ErrorException, errorMessage);
                throw new Exception(errorMessage, response.ErrorException);
            }

            return response.Data;
        }

        #endregion
    }
}
