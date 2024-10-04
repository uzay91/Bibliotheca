using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Core.Abstract;
using DataAccess;
using Microsoft.EntityFrameworkCore;



namespace Core.DataAccess.EntityFramework
{
    public class EfEntityRepository<TEntity, TContext> : IEntityRepository<TEntity> where TEntity : class, IEntity where TContext : DbContext
    {
        public EfEntityRepository(TContext context)
        {
            Context = context;
        }
        protected TContext Context { get; }

        public TEntity Add(TEntity entity)
        {
            return Context.Add(entity).Entity;
        }
        public TEntity Update(TEntity entity)
        {
            return Context.Update(entity).Entity;
        }

        public void Delete(TEntity entity)
        {
            Context.Remove(entity);
        }

        public TEntity Get(Expression<Func<TEntity, bool>> expression)
        {
            return Context.Set<TEntity>().FirstOrDefault(expression);
        }

        public async Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> expression)
        {
            return await Context.Set<TEntity>().AsQueryable().FirstOrDefaultAsync(expression);
        }

        public IEnumerable<TEntity> GetList(Expression<Func<TEntity, bool>>? expression = null)
        {
            //inline if statement
            return expression == null 
                ? Context.Set<TEntity>().AsNoTracking() 
                : Context.Set<TEntity>().Where(expression).AsNoTracking();
        }

        public async Task<IEnumerable<TEntity>> GetListAsync(Expression<Func<TEntity, bool>>? expression = null)
        {
            return expression == null
                ? await Context.Set<TEntity>().ToListAsync()
                : await Context.Set<TEntity>().Where(expression).ToListAsync();
        }

        public int SaveChanges()
        {
            return Context.SaveChanges();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await Context.SaveChangesAsync();
        }

        public IQueryable<TEntity> Query()
        {
            return Context.Set<TEntity>();
        }
        public async Task<int> GetCountAsync(Expression<Func<TEntity, bool>>? expression = null)
        {
            if (expression == null)
            {
                return await Context.Set<TEntity>().CountAsync();
            }
            else
            {
                return await Context.Set<TEntity>().CountAsync(expression);
            }
        }

    }
}
