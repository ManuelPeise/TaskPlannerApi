using Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Data.Accessor.Interfaces
{
    public interface IDbRepositoryBase<TEntity> where TEntity: AEntityBase
    {
        Task<HashSet<TEntity>> GetAll(
            bool asNoTracking = false,
            params Func<IQueryable<TEntity>, IQueryable<TEntity>>[]? includes);

        Task<HashSet<TEntity>> GetBy(
            Expression<Func<TEntity, bool>> predicate,
            bool asNoTracking = false,
            params Func<IQueryable<TEntity>, IQueryable<TEntity>>[]? includes);

        Task<HashSet<TEntity>> GetById(
            int id,
            bool asNoTracking = false,
            params Func<IQueryable<TEntity>, IQueryable<TEntity>>[]? includes);

        Task Insert(TEntity entity, Expression<Func<TEntity, bool>>? predicate);
     
        Task Delete(int id);
    }
}
