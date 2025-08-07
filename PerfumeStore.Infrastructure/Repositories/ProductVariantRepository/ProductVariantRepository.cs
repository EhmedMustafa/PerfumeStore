using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PerfumeStore.Application.Interfaces.IProductVariant;
using PerfumeStore.Infrastructure.Data;

namespace PerfumeStore.Infrastructure.Repositories.ProductVariant
{
    public class ProductVariantRepository:IProductVariantRepository
    {
        private readonly AppDbContext _appDbContext;

        public ProductVariantRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<Domain.Entities.ProductVariant> GetVariantWithDetailsByIdAsync(int id)
        {
            var variant = await _appDbContext.ProductVariants
               .Include(p => p.Product) // Bu naviqasiya property-dir, bu qalır
               .FirstOrDefaultAsync(v => v.Id == id);
            if (variant == null)
                Console.WriteLine($"Variant tapılmadı: ID = {id}");
            else
                Console.WriteLine($"Variant tapıldı: ID = {variant.Id}, Size = {variant.Size}");

            return variant;
        }
    }
}
