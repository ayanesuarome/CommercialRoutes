using Imperial.CommercialRoutes.Domain.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Imperial.CommercialRoutes.Domain.Interfaces
{
    /// <summary>
    /// Base interface for creating entities.
    /// </summary>
    /// <typeparam name="TEntity">Type of the entity.</typeparam>
    public interface ICreate<TEntity>
        where TEntity : class, IEntity
    {
        /// <summary>
        /// Inserts an entity.
        /// </summary>
        /// <param name="entity">Entity to insert.</param>
        /// <param name="commandTimeout">The command timeout (in seconds).</param>
        /// <returns>Entity inserted.</returns>
        Task<TEntity> InsertAsync(TEntity entity, int? commandTimeout = null);

        /// <summary>
        /// Inserts a list of entities.
        /// </summary>
        /// <param name="entities">Entities to insert.</param>
        /// <param name="commandTimeout">The command timeout (in seconds).</param>
        /// <returns>Number of rows inserted.</returns>
        Task<int> InsertAsync(IList<TEntity> entities, int? commandTimeout = null);
    }
}
