using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Imperial.CommercialRoutes.Domain.Entities;
using Imperial.CommercialRoutes.Domain.Entities.Models;
using Imperial.CommercialRoutes.Domain.Interfaces;
using Imperial.CommercialRoutes.Infrastructure.DatabaseContext;
using Serilog;

namespace Imperial.CommercialRoutes.Infrastructure.Repositories
{
    /// <summary>
    /// Distance repository implementation.
    /// </summary>
    public class DistanceRepository : DapperRepository<Distance>, IDistanceRepository
    {
        #region Fields

        private const string DistanceTableName = "Distance";

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new inctance of the <see cref="DistanceRepository"/> class.
        /// </summary>
        /// <param name="logger">Logger instance.</param>
        /// <param name="dbContext">Data model instance</param>
        public DistanceRepository(
            ILogger logger,
            CommercialRoutesDbContext dbContext)
            : base(logger, dbContext)
        {
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets distance by query.
        /// </summary>
        /// <param name="options">Options to search specific distance.</param>
        /// <param name="cancellationToken">Used to propagate notifications that the operation should be canceled.</param>
        /// <returns>Distance.</returns>
        public async Task<Distance> GetDistanceAsync(
            DistanceSearchOptions options = null,
            CancellationToken cancellationToken = default)
        {
            Dictionary<string, object> filters = new Dictionary<string, object>();

            if (options?.Origin != null)
            {
                filters.Add(nameof(Distance.Origin), options.Origin);
            }
            if (options?.Destination != null)
            {
                filters.Add(nameof(Distance.Destination), options.Destination);
            }
            if (options?.LunarYears != null)
            {
                filters.Add(nameof(Distance.LunarYears), options.LunarYears);
            }

            StringBuilder select = new StringBuilder($"SELECT * FROM {DistanceTableName} WHERE 1 = 1");
            DynamicParameters queryParams = new DynamicParameters();

            foreach (var filter in filters)
            {
                queryParams.Add(filter.Key, filter.Value);
                select.Append($" AND {filter.Key} = @{filter.Key}");
            }

            return await GetFirstOrDefaultAsync(select.ToString(), queryParams);
        }

        /// <summary>
        /// Gets distances by query.
        /// </summary>
        /// <param name="options">Options to search specific distances.</param>
        /// <param name="cancellationToken">Used to propagate notifications that the operation should be canceled.</param>
        /// <returns>List of distances.</returns>
        public async Task<IList<Distance>> GetDistancesAsync(
            DistanceSearchOptions options = null,
            CancellationToken cancellationToken = default)
        {
            Dictionary<string, object> filters = new Dictionary<string, object>();

            if (options?.Origin != null)
            {
                filters.Add(nameof(Distance.Origin), options.Origin);
            }
            if (options?.Destination != null)
            {
                filters.Add(nameof(Distance.Destination), options.Destination);
            }
            if (options?.LunarYears != null)
            {
                filters.Add(nameof(Distance.LunarYears), options.LunarYears);
            }

            StringBuilder select = new StringBuilder($"SELECT * FROM {DistanceTableName} WHERE 1 = 1");
            DynamicParameters queryParams = new DynamicParameters();

            foreach (var filter in filters)
            {
                queryParams.Add(filter.Key, filter.Value);
                select.Append($" AND {filter.Key} = @{filter.Key}");
            }

            return await GetAsync(select.ToString(), queryParams);
        }

        #endregion
    }
}