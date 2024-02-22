using Imperial.CommercialRoutes.Domain.Interfaces;
using System.Threading.Tasks;

namespace Imperial.CommercialRoutes.Domain.Interfaces
{
    /// <summary>
    /// Base interface for updating entities.
    /// </summary>
    /// <typeparam name="TEntity">Type of the entity.</typeparam>
    public interface IUpdate<TEntity>
        where TEntity : class, IEntity
    {
        /// <summary>
        /// Updates an entity.
        /// </summary>
        /// <param name="entity">Entities to update.</param>
        /// <param name="commandTimeout">The command timeout (in seconds).</param>
        /// <returns>Entity updated</returns>
        Task<TEntity> UpdateAsync(TEntity entity, int? commandTimeout = null);
    }
}
