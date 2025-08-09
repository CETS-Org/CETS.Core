using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Interfaces
{
    public interface IBaseRepository<T> where T : class
    {
        IQueryable<T> GetQueryable(params Expression<Func<T, object>>[] includes);

        Task<T?> GetByIdAsync(int id, bool asNoTracking = false);

        Task<IEnumerable<T>> GetAllAsync();

        Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> predicate);

        Task<T?> FindFirstAsync(Expression<Func<T, bool>> predicate, bool AsNoTracking = false);

        Task AddAsync(T entity);

        Task AddRangeAsync(IEnumerable<T> entities);

        void Update(T entity);

        void Delete(T entity);

        Task DeleteByIdAsync(int id);

        void DeleteRange(IEnumerable<T> entities);

        Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate);

        Task<bool> ExistsByIdAsync(int id);

        Task<int> CountAsync();

        Task<int> CountAsync(Expression<Func<T, bool>> predicate);
    }
}
