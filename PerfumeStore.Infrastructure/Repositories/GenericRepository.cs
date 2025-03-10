using Microsoft.EntityFrameworkCore;
using PerfumeStore.Application.Interfaces;
using PerfumeStore.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PerfumeStore.Infrastructure.Repositories
{
   public class GenericRepository <T>:IGenericRepository<T> where T : class
    {
        private readonly AppDbContext _appDbContext;
        private readonly DbSet<T> _dbset;

        public GenericRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
            _dbset = appDbContext.Set<T>();
        }

        public async Task AddAsync(T entity)
        {
            await _dbset.AddAsync(entity);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity= await _dbset.FindAsync(id);
            if (entity!=null)
            {
                _dbset.Remove(entity);
                await _appDbContext.SaveChangesAsync();
            }

        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return  await _dbset.ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
           return await _dbset.FindAsync(id);

        }

        public async Task UpdateAsync(T entity)
        {
             _dbset.FindAsync(entity);
            await _appDbContext.SaveChangesAsync();
        }
    }
}
