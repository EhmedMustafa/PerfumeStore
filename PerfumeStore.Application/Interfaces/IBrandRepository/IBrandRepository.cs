using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PerfumeStore.Domain.Entities;

namespace PerfumeStore.Application.Interfaces.IBrandRepository
{
    public interface IBrandRepository
    {
        Task<Brand> GetBrandWithProductsByIdAsync(int brandId);
    }
}
