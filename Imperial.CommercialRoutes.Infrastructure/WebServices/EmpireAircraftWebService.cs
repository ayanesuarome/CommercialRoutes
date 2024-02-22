using Imperial.CommercialRoutes.Application.DTOs;
using Imperial.CommercialRoutes.Application.Interfaces.WebServices;
using RestSharp;
using Serilog;
using System;
using System.Configuration;

namespace Imperial.CommercialRoutes.Infrastructure.WebServices
{
    /// <summary>
    /// Service implementation for empire aircraft service.
    /// </summary>
    public class EmpireAircraftWebService : BaseWebService, IEmpireAircraftWebService
    {
        #region Fields

        // The abstraction does not allow to execute non-asynchronous operations and to consume this API does not support async.
        private readonly RestClient _client;
        private string EmpireAircraftsAddress => ConfigurationManager.AppSettings.Get("EmpireAircraftsAddress");

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the class <see cref="EmpireAircraftWebService"/>.
        /// </summary>
        /// <param name="logger">Logger instance.</param>
        /// <param name="client">HTTP client instance.</param>
        public EmpireAircraftWebService(
            ILogger logger,
            RestClient client)
            : base(logger, client)
        {
            _client = client;
        }
            
        /// <summary>
        /// Gets aircrafts.
        /// </summary>
        /// <returns>Aircrafts.</returns>
        public EmpireAircraft GetAircrafts()
        {
            RestRequest request = GetRestRequest(EmpireAircraftsAddress, Method.Get);
            var response = _client.Execute<EmpireAircraft>(request);

            if (!response.IsSuccessful)
            {
                Logger.Warning("Could not get the list of aircrafts. Fault response code: {0}.", response.StatusCode);
            }
            if (response.ErrorException != null)
            {
                string errorMessage = "Could not get the list of aircrafts.";
                Logger.Error(response.ErrorException, errorMessage);
                throw new Exception(errorMessage, response.ErrorException);
            }

            return response.Data;
        }

        #endregion
    }
}
