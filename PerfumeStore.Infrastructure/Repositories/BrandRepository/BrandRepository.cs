using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PerfumeStore.Application.Interfaces.IBrandRepository;
using PerfumeStore.Domain.Entities;
using PerfumeStore.Infrastructure.Data;

namespace PerfumeStore.Infrastructure.Repositories.Repository
{
    public class BrandRepository : IBrandRepository
    {
        private readonly AppDbContext _appDbContext;

        public BrandRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<IEnumerable<Brand>> GetAllAsync()
        {
            return await _appDbContext.Brands
                .OrderBy(b => b.Name)
                .ToListAsync();
        }

        public async Task<Brand> GetBrandWithProductsByIdAsync(int brandId)
        {
            return await _appDbContext.Brands
                .Include(b => b.products)
                .ThenInclude(p => p.Category)
                .ThenInclude(pv=>pv.Products)
                .ThenInclude(pv=>pv.ProductVariants)
                .FirstOrDefaultAsync(b => b.Id == brandId);
        }
    }
}
