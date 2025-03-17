using PerfumeStore.Application.Dtos.ProductDtos;
using PerfumeStore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PerfumeStore.Application.Services
{
    public interface IProductService
    {
        Task<IEnumerable<ResultProductDto>> GetAllAsync();
        Task<ResultProductDto> GetByIdAsync(int id);
        Task AddAsync(ProductCreateDto productdto);
        Task UpdateAsync(ProductUpdateDto model);
        Task DeleteAsync(int id);
        Task<IEnumerable<Product>> FindAsync(Expression<Func<Product, bool>> predicate);
        Task<List<ResultProductDto>> GetProductTake(int count);
        Task<List<ResultProductDto>> GetProductBySearch(string search);
        Task<List<ResultProductDto>> GetProductByPriceFilter(decimal minprice, decimal maxprice);
        Task<List<ResultProductDto>> GetProductByCategory(int categoryId);

    }
}

