using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PerfumeStore.Application.Interfaces
{
    public interface  IGenericRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(int id);
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
        Task<int> SaveChangesAsync();  // <-- Bunu əlavə edirik
        Task<List<T>> GetTakeAsync(int count);
        Task<T> GetByIdWithIncludeAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes);
    }
}
