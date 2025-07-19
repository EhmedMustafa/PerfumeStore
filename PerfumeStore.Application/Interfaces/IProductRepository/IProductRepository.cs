using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PerfumeStore.Application.Dtos.ProductDtos;
using PerfumeStore.Domain.Entities;

namespace PerfumeStore.Application.Interfaces.IProductRepository
{
    public interface IProductRepository
    {
        Task<List<Product>> GetProductByCategory(int? categoryId);
        Task<List<Product>> GetProductByPriceFilter(decimal minprice, decimal maxprice);
        Task<List<Product>> GetProductBySearch(string search);
        Task<List<Product>> GetAllWithNotesAsync();
        Task<List<Product>> GetDailyRotatedProductsAsync(int categoryId, int count);
        Task <List<Product>> GetBestsellerProductsAsync(int count);
        Task <List<Product>> GetNewProductsAsync();
        Task<PaginatedResult<Product>> GetPagedProductsAsync(List<int> categoryIds, int? brandId, int? fragranceFamilyId, int page, int pageSize);
        Task<PaginatedResult<Product>> GetPagedProductsByCategoryAsync(int categoryId, int page, int pageSize);
       

    }
}
