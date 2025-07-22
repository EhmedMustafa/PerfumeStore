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

        public async Task<Brand> GetBrandWithProductsByIdAsync(int brandId)
        {
            return await _appDbContext.Brands
                .Include(b => b.products)
                .FirstOrDefaultAsync(b => b.Id == brandId);
        }
    }
}
