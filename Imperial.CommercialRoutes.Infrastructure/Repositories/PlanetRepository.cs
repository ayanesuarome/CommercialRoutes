using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using DapperQueryBuilder;
using Imperial.CommercialRoutes.Domain.Entities;
using Imperial.CommercialRoutes.Domain.Entities.Models;
using Imperial.CommercialRoutes.Domain.Interfaces;
using Imperial.CommercialRoutes.Infrastructure.DatabaseContext;
using Serilog;

namespace Imperial.CommercialRoutes.Infrastructure.Repositories
{
    /// <summary>
    /// Planet repository implementation.
    /// </summary>
    public class PlanetRepository : DapperRepository<Planet>, IPlanetRepository
    {
        #region Fields

        private const string PlanetTableName = "Planet";

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new inctance of the <see cref="PlanetRepository"/> class.
        /// </summary>
        /// <param name="logger">Logger instance.</param>
        /// <param name="dbContext">Data model instance</param>
        public PlanetRepository(
            ILogger logger,
            CommercialRoutesDbContext dbContext)
            : base(logger, dbContext)
        {
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets planet by query.
        /// </summary>
        /// <param name="options">Options to search specific planet.</param>
        /// <param name="cancellationToken">Used to propagate notifications that the operation should be canceled.</param>
        /// <returns>Planet.</returns>
        public async Task<Planet> GetPlanetAsync(
            PlanetSearchOptions options = null,
            CancellationToken cancellationToken = default)
        {
            Dictionary<string, object> filters = new Dictionary<string, object>();
            
            if (options?.Name != null)
            {
                filters.Add(nameof(Planet.Name), options.Name);
            }
            if (options?.Code != null)
            {
                filters.Add(nameof(Planet.Code), options.Code);
            }
            if (options?.Sector != null)
            {
                filters.Add(nameof(Planet.Sector), options.Sector);
            }

            StringBuilder select = new StringBuilder($"SELECT * FROM {PlanetTableName} WHERE 1 = 1");
            DynamicParameters queryParams = new DynamicParameters();

            foreach (var filter in filters)
            {
                queryParams.Add(filter.Key, filter.Value);
                select.Append($" AND {filter.Key} = @{filter.Key}");
            }

            return await GetFirstOrDefaultAsync(select.ToString(), queryParams);
        }

        /// <summary>
        /// Gets planets by query.
        /// </summary>
        /// <param name="options">Options to search specific planets.</param>
        /// <param name="cancellationToken">Used to propagate notifications that the operation should be canceled.</param>
        /// <returns>List of planets.</returns>
        public async Task<IList<Planet>> GetPlanetsAsync(
            PlanetSearchOptions options = null,
            CancellationToken cancellationToken = default)
        {
            Dictionary<string, object> filters = new Dictionary<string, object>();

            if (options?.Name != null)
            {
                filters.Add(nameof(Planet.Name), options.Name);
            }
            if (options?.Code != null)
            {
                filters.Add(nameof(Planet.Code), options.Code);
            }
            if (options?.Sector != null)
            {
                filters.Add(nameof(Planet.Sector), options.Sector);
            }
            if (options?.RebelInfluence != null)
            {
                filters.Add(nameof(Planet.RebelInfluence), options.RebelInfluence);
            }

            StringBuilder select = new StringBuilder($"SELECT * FROM {PlanetTableName} WHERE 1 = 1");
            DynamicParameters queryParams = new DynamicParameters();

            foreach (var filter in filters)
            {
                queryParams.Add(filter.Key, filter.Value);
                select.Append($" AND {filter.Key} = @{filter.Key}");
            }
            return await GetAsync(select.ToString(), queryParams);
        }

        /// <summary>
        /// Gets planets by names.
        /// </summary>
        /// <param name="names">Names of planets to search for.</param>
        /// <param name="cancellationToken">Used to propagate notifications that the operation should be canceled.</param>
        /// <returns>List of planets matching the provided names.</returns>
        public async Task<IList<Planet>> GetPlanetsByNamesAsync(
            string[] names,
            CancellationToken cancellationToken = default)
        {
            using (IDbConnection connection = DbContext.Connection)
            {
                try
                {
                    QueryBuilder query = connection.QueryBuilder($"SELECT * FROM Planet WHERE 1 = 1");
                    query.Append($"AND Name = {names[0]}");

                    for (int i = 1; i < names.Length; i++)
                    {
                        query.Append($"OR Name = {names[i]}");
                    }

                    var results = await query.QueryAsync<Planet>(commandTimeout: DefaultCommandTimeout);
                    return results?.ToList();
                }
                catch (Exception ex)
                {
                    Logger.Error(ex, "Could not get the list of entities from the database.");
                    return (IList<Planet>)new List<Planet>();
                }
            }
        }

        #endregion
    }
}