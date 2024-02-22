using Imperial.CommercialRoutes.Domain.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Imperial.CommercialRoutes.Domain.Interfaces
{
    /// <summary>
    /// Base interface for seeking entities.
    /// </summary>
    /// <typeparam name="TEntity">Type of the entity</typeparam>
    public interface IRead<TEntity>
        where TEntity : class, IEntity
    {
        /// <summary>
        /// Gets all entities by a query.
        /// </summary>
        /// <param name="sql">The SQL to execute for the query.</param>
        /// <param name="param">The parameters to pass, if any.</param>
        /// <param name="commandTimeout">The command timeout (in seconds).</param>
        /// <returns>List of entities</returns>
        Task<IList<TEntity>> GetAsync(
            string sql,
            object param = null,
            int? commandTimeout = null);

        /// <summary>
        /// Gets entity by a query.
        /// </summary>
        /// <param name="sql">The SQL to execute for the query.</param>
        /// <param name="param">The parameters to pass, if any.</param>
        /// <param name="commandTimeout">The command timeout (in seconds).</param>
        /// <returns>Entity</returns>
        Task<TEntity> GetFirstOrDefaultAsync(
            string sql,
            object param = null,
            int? commandTimeout = null);
    }
}
