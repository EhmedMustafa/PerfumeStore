using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using PerfumeStore.Application.Interfaces;
using PerfumeStore.Application.Services;
using PerfumeStore.Domain.Entities;

namespace PerfumeStore.Infrastructure.Services
{
    public class ProductFeatureService : IProductFeatureService
    {
        private readonly IGenericRepository<ProductFeature> _productFeatureRepository;

        public ProductFeatureService(IGenericRepository<ProductFeature> productFeatureRepository)
        {
            _productFeatureRepository = productFeatureRepository;
        }

        public async Task<IEnumerable<ProductFeature>> GetAllAsync()
        {
            return await _productFeatureRepository.GetAllAsync();
        }

        public async Task<ProductFeature> GetByIdAsync(int id)
        {
            return await _productFeatureRepository.GetByIdAsync(id);
        }

        public async Task AddAsync(ProductFeature productFeature)
        {
            await _productFeatureRepository.AddAsync(productFeature);
            await _productFeatureRepository.SaveChangesAsync();
        }

        public async Task UpdateAsync(ProductFeature productFeature)
        {
            await _productFeatureRepository.UpdateAsync(productFeature);
            await _productFeatureRepository.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var productFeature = await _productFeatureRepository.GetByIdAsync(id);
            if (productFeature != null)
            {
                await _productFeatureRepository.DeleteAsync(productFeature);
                await _productFeatureRepository.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<ProductFeature>> FindAsync(Expression<Func<ProductFeature, bool>> predicate)
        {
            return await _productFeatureRepository.FindAsync(predicate);
        }
    }
}
