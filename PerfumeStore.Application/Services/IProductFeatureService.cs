using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PerfumeStore.Domain.Entities;

namespace PerfumeStore.Application.Services
{
    public interface IProductFeatureService
    {
        Task<IEnumerable<ProductFeature>> GetAllAsync();
        Task<ProductFeature> GetByIdAsync(int id);
        Task AddAsync(ProductFeature productFeature);
        Task UpdateAsync(ProductFeature productFeature);
        Task DeleteAsync(int id);
    }
}
