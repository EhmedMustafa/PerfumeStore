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
        Task<List<Product>> GetProductByPriceFilter(int? Minprice, int? Maxprice);
        Task<List<Product>> GetProductBySearch(string search);
        Task<List<Product>> GetAllWithNotesAsync();
        Task<List<Product>> GetDailyRotatedProductsAsync(int categoryId, int count);
        Task <List<Product>> GetBestsellerProductsAsync(int count);
        Task <List<Product>> GetNewProductsAsync();
        Task<Product> GetProductByIdWithNotes(int id);

        Task <Product> DeleteProduct(int Id);
        Task<PaginatedResult<Product>> GetPagedProductsAsync(List<int> categoryIds, List<int> brandId, List<int> fragranceFamilyId,List<int> FragranceNoteId, int page, int pageSize,int? minPrice,int? maxPrice);
        Task<PaginatedResult<Product>> GetPagedProductsByCategoryAsync(int categoryId, int page, int pageSize);
       

    }
}
