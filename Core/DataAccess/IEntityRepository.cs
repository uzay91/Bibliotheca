using Core.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    // Repository Pattern 
    public interface IEntityRepository<T> where T : class, IEntity
    {
        T Add(T entity);
        T Update(T entity);
        void Delete(T entity);
        IEnumerable<T> GetList(Expression<Func<T, bool>>? expression = null);
        Task<IEnumerable<T>> GetListAsync(Expression<Func<T, bool>>? expression = null);
        T Get(Expression<Func<T, bool>> expression);
        Task<T> GetAsync(Expression<Func<T, bool>> expression);
        int SaveChanges();
        Task<int> SaveChangesAsync();
        IQueryable<T> Query();
        Task<int> GetCountAsync(Expression<Func<T, bool>>? expression = null);

    }
}
