using Models.Interfaces.Core;
using System.Linq;

namespace Infra.Data.Interfaces.Core
{
    public interface IRepository<TEntity> : IQueryable<TEntity>
        where TEntity : IEntity
    {
        void Add(TEntity entity);
        void Update(TEntity entity);
        void Delete(TEntity entity);
    }
}
