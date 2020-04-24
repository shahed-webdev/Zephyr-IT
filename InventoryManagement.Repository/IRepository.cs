using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace InventoryManagement.Repository
{
    public interface IRepository<TEntity> where TEntity : class
    {
        TEntity Find(int id);
        IEnumerable<TEntity> ToList();
        IEnumerable<TEntity> Where(Expression<Func<TEntity, bool>> predicate);
        TEntity SingleOrDefault(Expression<Func<TEntity, bool>> predicate);
        bool Any(Expression<Func<TEntity, bool>> predicate);
        void Add(TEntity entity);
        void AddRange(IEnumerable<TEntity> entities);
        void Remove(TEntity entity);
        void RemoveRange(IEnumerable<TEntity> entities);
        void Update(TEntity entity);
        void UpdateRange(IEnumerable<TEntity> entities);



        Task<TEntity> FindAsync(int id);
        Task<IEnumerable<TEntity>> ToListAsync();
        Task<IEnumerable<TEntity>> WhereAsync(Expression<Func<TEntity, bool>> predicate);
        Task<TEntity> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> predicate);
        Task AnyAsync(Expression<Func<TEntity, bool>> predicate);

    }
}
