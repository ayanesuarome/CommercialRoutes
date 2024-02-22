using Imperial.CommercialRoutes.Application.DTOs;
using Imperial.CommercialRoutes.Domain.Entities.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Imperial.CommercialRoutes.Application.Interfaces.WebServices
{
    /// <summary>
    /// Service definition for empire spy report.
    /// </summary>
    public interface IEmpireRebelInfluenceWebService
    {
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
        Task<IList<Rebel>> GetSpyReportAsync(
            RebelSearchOptions options = null,
            CancellationToken cancellationToken = default);
    }
}
