using Imperial.CommercialRoutes.Domain.Interfaces;

namespace Imperial.CommercialRoutes.Domain.Interfaces
{
    /// <summary>
    /// Base repository definition that all repositories must inherit.
    /// </summary>
    /// <typeparam name="TEntity">Type of the entity</typeparam>
    public interface IRepository<TEntity>
        where TEntity : class, IEntity
    {
    }
}
