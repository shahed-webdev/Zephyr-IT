using InventoryManagement.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace InventoryManagement.Repository
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected readonly ApplicationDbContext Context;

        public Repository(ApplicationDbContext context)
        {
            Context = context;
        }

        public TEntity Find(int id)
        {
            return Context.Set<TEntity>().Find(id);
        }

        public TT FindGeneric<TT>(int id) where TT : class
        {
            return Context.Set<TT>().Find(id);
        }


        IEnumerable<TEntity> IRepository<TEntity>.ToList()
        {
            return ToList();
        }

        public IEnumerable<TEntity> Where(Expression<Func<TEntity, bool>> predicate)
        {
            return Context.Set<TEntity>().Where(predicate);
        }

        TEntity IRepository<TEntity>.SingleOrDefault(Expression<Func<TEntity, bool>> predicate)
        {
            return SingleOrDefault(predicate);
        }

        bool IRepository<TEntity>.Any(Expression<Func<TEntity, bool>> predicate)
        {
            return Any(predicate);
        }

        public IEnumerable<TEntity> ToList()
        {
            return Context.Set<TEntity>().ToList();
        }

        public TEntity SingleOrDefault(Expression<Func<TEntity, bool>> predicate)
        {
            return Context.Set<TEntity>().SingleOrDefault(predicate);
        }

        public bool Any(Expression<Func<TEntity, bool>> predicate)
        {
            return Context.Set<TEntity>().Any(predicate);
        }

        public void Add(TEntity entity)
        {
            Context.Set<TEntity>().Add(entity);
        }

        void IRepository<TEntity>.AddRange(IEnumerable<TEntity> entities)
        {
            AddRange(entities);
        }

        public void AddRange(IEnumerable<TEntity> entities)
        {
            Context.Set<TEntity>().AddRange(entities);
        }

        public void Remove(TEntity entity)
        {
            Context.Set<TEntity>().Remove(entity);
        }

        void IRepository<TEntity>.RemoveRange(IEnumerable<TEntity> entities)
        {
            RemoveRange(entities);
        }

        public void RemoveRange(IEnumerable<TEntity> entities)
        {
            Context.Set<TEntity>().RemoveRange(entities);
        }

        public void Update(TEntity entity)
        {
            Context.Set<TEntity>().Update(entity);
        }

        void IRepository<TEntity>.UpdateRange(IEnumerable<TEntity> entities)
        {
            UpdateRange(entities);
        }

        Task<TEntity> IRepository<TEntity>.FindAsync(int id)
        {
            return FindAsync(id);
        }

        Task<IEnumerable<TEntity>> IRepository<TEntity>.ToListAsync()
        {
            return ToListAsync();
        }

        Task<IEnumerable<TEntity>> IRepository<TEntity>.WhereAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return WhereAsync(predicate);
        }

        Task<TEntity> IRepository<TEntity>.SingleOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return SingleOrDefaultAsync(predicate);
        }

        Task IRepository<TEntity>.AnyAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return AnyAsync(predicate);
        }

        public void UpdateRange(IEnumerable<TEntity> entities)
        {
            Context.Set<TEntity>().UpdateRange(entities);
        }

        //Async and await
        public async Task<TEntity> FindAsync(int id)
        {
            return await Context.Set<TEntity>().FindAsync(id).ConfigureAwait(false);
        }

        public async Task<IEnumerable<TEntity>> ToListAsync()
        {
            return await Context.Set<TEntity>().ToListAsync().ConfigureAwait(false);
        }

        public async Task<IEnumerable<TEntity>> WhereAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await Context.Set<TEntity>().Where(predicate).ToListAsync().ConfigureAwait(false);
        }

        public async Task<TEntity> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await Context.Set<TEntity>().SingleOrDefaultAsync(predicate).ConfigureAwait(false);
        }

        public Task AnyAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return Context.Set<TEntity>().AnyAsync(predicate);
        }

    }
}
