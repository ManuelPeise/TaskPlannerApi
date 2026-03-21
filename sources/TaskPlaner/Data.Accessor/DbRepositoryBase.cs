using System.Linq.Expressions;
using Data.Accessor.Interfaces;
using Data.Database;
using Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data.Accessor
{
    public class DbRepositoryBase<TEntity> : IDbRepositoryBase<TEntity> where TEntity : AEntityBase
    {
        private readonly DatabaseContext _dbContext;

        public DbRepositoryBase(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<HashSet<TEntity>> GetAll(
            bool asNoTracking = false,
            params Func<IQueryable<TEntity>, IQueryable<TEntity>>[]? includes)
        {
            IQueryable<TEntity> table = asNoTracking
                ? _dbContext.Set<TEntity>().AsNoTracking()
                : _dbContext.Set<TEntity>();

            if (includes != null)
            {
                foreach (var includeExpression in includes)
                {
                    if (includeExpression != null)
                    {
                        table = includeExpression(table);
                    }
                }
            }

            return await table.ToHashSetAsync();
        }

        public async Task<HashSet<TEntity>> GetBy(
            Expression<Func<TEntity, bool>> predicate,
            bool asNoTracking = false,
            params Func<IQueryable<TEntity>, IQueryable<TEntity>>[]? includes)
        {
            IQueryable<TEntity> table = asNoTracking
                ? _dbContext.Set<TEntity>().AsNoTracking()
                : _dbContext.Set<TEntity>();

            if (includes != null)
            {
                foreach (var includeExpression in includes)
                {
                    if (includeExpression != null)
                    {
                        table = includeExpression(table);
                    }
                }
            }

            table = table.Where(predicate);

            return await table.ToHashSetAsync();
        }

        public async Task<HashSet<TEntity>> GetById(
            int id,
            bool asNoTracking = false,
            params Func<IQueryable<TEntity>, IQueryable<TEntity>>[]? includes)
        {
            IQueryable<TEntity> table = asNoTracking
                ? _dbContext.Set<TEntity>().AsNoTracking()
                : _dbContext.Set<TEntity>();

            if (includes != null)
            {
                foreach (var includeExpression in includes)
                {
                    if (includeExpression != null)
                    {
                        table = includeExpression(table);
                    }
                }
            }

            return await table.Where(e => e.Id == id).ToHashSetAsync();
        }

        public async Task Insert(TEntity entity, Func<TEntity, bool> predicate)
        {
            var table = _dbContext.Set<TEntity>().AsNoTracking();
            var existingEntity = table.FirstOrDefault(predicate);

            if (existingEntity == null)
            {
                _dbContext.Set<TEntity>().Add(entity);
            }
        }

        public async Task Delete(int id)
        {
            var entity = await _dbContext.Set<TEntity>().FindAsync(id);

            if (entity != null)
            {
                _dbContext.Set<TEntity>().Remove(entity);
            }
        }
    }
}
