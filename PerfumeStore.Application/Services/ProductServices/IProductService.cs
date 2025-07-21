using PerfumeStore.Application.Dtos.ProductDtos;
using PerfumeStore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PerfumeStore.Application.Services.ProductServices
{
    public interface IProductService
    {
        Task<IEnumerable<ResultProductDto>> GetAllProductAsync();
        Task<GetByIdProductDto> GetByIdProductAsync(int id);
        Task CreateProductAsync(CreateProductDto productdto);
        Task UpdateProductAsync(UpdateProductDto model);
        Task DeleteProductAsync(int id);

        Task<List<ResultProductDto>> GetAllWithNotesAsync();
        Task<List<ResultProductDto>> GetDailyRotatedProductsAsync(int categoryId, int count);

        Task<List<ResultProductDto>> GetBestsellerProductsAsync(int count);

        Task<List<ResultProductDto>> GetNewProductsAsync();

        Task<PaginatedResult<ResultProductDto>> GetPagedProductsAsync(List<int> categoryIds, List<int> brandId, List<int> fragranceFamilyId, List<int> fragranceNoteId, int page, int pageSize,int? minPrice,int? maxPrice);

        Task<PaginatedResult<ResultProductDto>> GetPagedProductsByCategoryAsync(int categoryId, int page, int pageSize);
        






        Task<IEnumerable<Product>> FindAsync(Expression<Func<Product, bool>> predicate);
        Task<List<ResultProductDto>> GetProductTake(int count);
        Task<List<ResultProductDto>> GetProductBySearch(string search);
        Task<List<ResultProductDto>> GetProductByPriceFilter(int? minPrice, int? maxPrice);
        Task<List<ResultProductDto>> GetProductByCategory(int? categoryId);
        
    }
}

