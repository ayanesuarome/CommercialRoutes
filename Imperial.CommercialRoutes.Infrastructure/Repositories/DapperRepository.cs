using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Dapper.Contrib.Extensions;
using Imperial.CommercialRoutes.Domain.Interfaces;
using Imperial.CommercialRoutes.Infrastructure.DatabaseContext;
using Serilog;

namespace Imperial.CommercialRoutes.Infrastructure.Repositories
{
    /// <summary>
    /// Base repository implementation that all repositories must inherit.
    /// </summary>
    public class DapperRepository<TEntity>
        : IRead<TEntity>, ICreate<TEntity>, IUpdate<TEntity>, IRepository<TEntity>, IDisposable
        where TEntity : class, IEntity
    {
        #region Fields

        private const string SqlCommandTimeoutInSeconds = "SqlCommandTimeoutInSeconds";
        protected readonly ILogger Logger;

        protected int DefaultCommandTimeout = 0;
        protected readonly CommercialRoutesDbContext DbContext;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new inctance of the <see cref="DapperRepository{TEntity}"/> class.
        /// </summary>
        /// <param name="logger">Logger instance.</param>
        /// <param name="dbContext">Data model instance</param>
        protected DapperRepository(
            ILogger logger,
            CommercialRoutesDbContext dbContext)
        {
            Logger = logger;
            DbContext = dbContext;
            
            Int32.TryParse(ConfigurationManager.AppSettings.Get(SqlCommandTimeoutInSeconds), out DefaultCommandTimeout);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets all entities by a query.
        /// </summary>
        /// <param name="sql">The SQL to execute for the query.</param>
        /// <param name="param">The parameters to pass, if any.</param>
        /// <param name="commandTimeout">The command timeout (in seconds).</param>
        /// <returns>List of entities</returns>
        public virtual async Task<IList<TEntity>> GetAsync(
            string sql,
            object param = null,
            int? commandTimeout = null)
        {
            using (IDbConnection connection = DbContext.Connection)
            {
                try
                {
                    var results = await connection.QueryAsync<TEntity>(
                        sql: sql,
                        param: param,
                        commandTimeout: commandTimeout ?? DefaultCommandTimeout);

                    return results?.ToList();
                }
                catch (Exception ex)
                {
                    Logger.Error(ex, "Could not get the list of entities from the database.");
                    return (IList<TEntity>)new List<TEntity>();
                }
            }
        }

        /// <summary>
        /// Gets entity by a query.
        /// </summary>
        /// <param name="sql">The SQL to execute for the query.</param>
        /// <param name="param">The parameters to pass, if any.</param>
        /// <returns>Entity</returns>
        public virtual async Task<TEntity> GetFirstOrDefaultAsync(
            string sql,
            object param = null,
            int? commandTimeout = null)
        {
            using (IDbConnection connection = DbContext.Connection)
            {
                try
                {
                    return await connection.QueryFirstOrDefaultAsync<TEntity>(
                        sql: sql,
                        param: param,
                        commandTimeout: commandTimeout ?? DefaultCommandTimeout);
                }
                catch (Exception ex)
                {
                    Logger.Error(ex, "Could not get the list of entities from the database.");
                    return null;
                }
            }
        }

        /// <summary>
        /// Inserts an entity.
        /// </summary>
        /// <param name="entity">Entity to insert.</param>
        /// <param name="commandTimeout">The command timeout (in seconds).</param>
        /// <returns>Entity inserted.</returns>
        public async Task<TEntity> InsertAsync(
            TEntity entity,
            int? commandTimeout = null)
        {
            using (IDbConnection connection = DbContext.Connection)
            {
                connection.Open();
                // start a transaction in case something goes wrong
                await connection.ExecuteAsync("BEGIN TRANSACTION");

                try
                {
                    // key will be created by the database
                    int id = (int)connection.Insert<TEntity>(entity);
                    entity.Id = id;
                    
                    await connection.ExecuteAsync("COMMIT TRANSACTION");

                    Logger.Information("{0} inserted with ID: {1}", typeof(TEntity).Name, entity.Id);
                    return entity;
                }
                catch (Exception ex)
                {
                    Logger.Error(ex, "Could not insert the entity into the database.");
                    await connection.ExecuteAsync("ROLLBAC TRANSACTION");
                    return null;
                }
            }
        }

        /// <summary>
        /// Inserts a list of entities.
        /// </summary>
        /// <param name="entities">Entities to insert.</param>
        /// <param name="commandTimeout">The command timeout (in seconds).</param>
        /// <returns>Number of rows inserted.</returns>
        public async Task<int> InsertAsync(
            IList<TEntity> entities,
            int? commandTimeout = null)
        {
            using (IDbConnection connection = DbContext.Connection)
            {
                connection.Open();
                // start a transaction in case something goes wrong
                await connection.ExecuteAsync("BEGIN TRANSACTION");

                try
                {
                    // key will be created by the database
                    int rowsAffected = (int)connection.Insert<IList<TEntity>>(entities, commandTimeout: commandTimeout ?? DefaultCommandTimeout);
                    
                    await connection.ExecuteAsync("COMMIT TRANSACTION");

                    Logger.Information("{0}(s) inserted: {1}", typeof(TEntity).Name, rowsAffected);
                    return rowsAffected;
                }
                catch (Exception ex)
                {
                    Logger.Error(ex, "Could not insert the entity into the database.");
                    await connection.ExecuteAsync("ROLLBAC TRANSACTION");
                    return 0;
                }
            }
        }

        /// <summary>
        /// Updates an entity.
        /// </summary>
        /// <param name="entity">Entities to update.</param>
        /// <param name="commandTimeout">The command timeout (in seconds).</param>
        /// <returns>Entity updated</returns>
        public async Task<TEntity> UpdateAsync(
            TEntity entity,
            int? commandTimeout = null)
        {
            using (IDbConnection connection = DbContext.Connection)
            {
                connection.Open();

                try
                {
                    await connection.UpdateAsync<TEntity>(entity, commandTimeout: commandTimeout);
                    return entity;
                }
                catch (Exception ex)
                {
                    Logger.Error(ex, "Could not get the list of entities from the database.");
                    return null;
                }
            }
        }

        #endregion

        #region Disposable Support

        private bool _disposeValue; // to detect redundant calls

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (_disposeValue) return;

            if (disposing)
            {
                DbContext.Connection.Dispose();
            }

            _disposeValue = true;
        }

        /// <summary>
        /// Finalize function to release unmanaged resources and performs other cleanup
        /// operations before the <see cref="Repository"/> class is reclaimed by garbage collection.
        /// </summary>
        ~DapperRepository()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}