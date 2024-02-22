using Imperial.CommercialRoutes.Application.DTOs;
using Imperial.CommercialRoutes.Application.Interfaces.WebServices;
using Imperial.CommercialRoutes.Domain.Entities.Models;
using RestSharp;
using Serilog;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Imperial.CommercialRoutes.Infrastructure.WebServices
{
    /// <summary>
    /// Service implementation for sindicate planet.
    /// </summary>
    public class EmpireRebelInfluenceWebService : BaseWebService, IEmpireRebelInfluenceWebService
    {
        #region Fields

        private string EmpireSpyReportAddress => ConfigurationManager.AppSettings.Get("EmpireSpyReportAddress");

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the class <see cref="EmpireRebelInfluenceWebService"/>.
        /// </summary>
        /// <param name="logger">Logger instance.</param>
        /// <param name="client">HTTP client instance.</param>
        public EmpireRebelInfluenceWebService(
            ILogger logger,
            IRestClient client)
            : base(logger, client)
        {
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets report of rebel influences in planets.
        /// </summary>
        /// <remarks>
        /// Parameter option is here only to simulate a scalable search filter because regardeless the current data set is short,
        /// millions of records should exist in the report and loading all of them at once is not scalable at all.
        /// </remarks>
        /// <param name="options">Options to search specific rebel influences.</param>
        /// <param name="cancellationToken">Used to propagate notifications that the operation should be canceled.</param>
        /// <returns>Report of rebel influences in planets.</returns>
        public async Task<IList<Rebel>> GetSpyReportAsync(
            RebelSearchOptions options = null,
            CancellationToken cancellationToken = default)
        {
            RestRequest request = GetRestRequest(EmpireSpyReportAddress, Method.Get);
            IEnumerable<Rebel> response = await ExecuteAsync<IEnumerable<Rebel>>(request, cancellationToken);

            // Applying search filter to simulate that the service supports OData or any other filtering support.
            // This should not be here if the service has supported filtering.
            return (
                        from rebel in response
                        where (options?.Code == null || rebel.Code == options.Code)
                        && (options?.RebelInfluence == null || rebel.RebelInfluence == options.RebelInfluence)
                        select rebel
                    )
                    .ToList();
        }

        #endregion
    }
}
