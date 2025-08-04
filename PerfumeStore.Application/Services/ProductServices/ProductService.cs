using AutoMapper;
using PerfumeStore.Application.Dtos.ProductDtos;
using PerfumeStore.Application.Dtos.ProductVariantDtos;
using PerfumeStore.Application.Interfaces;
using PerfumeStore.Application.Interfaces.IProductRepository;
using PerfumeStore.Application.Services.ProductServices;
using PerfumeStore.Domain.Entities;
using System;
using System.Linq.Expressions;

namespace PerfumeStore.Infrastructure.Services
{
    public class ProductService : IProductService
    {
        private readonly IGenericRepository<Product> _productRepository;
        private readonly IMapper _mapper;
        private readonly IProductRepository _repository;
       

        public ProductService(IGenericRepository<Product> productRepository,IMapper mapper, IProductRepository _Repository)
        {
            _productRepository = productRepository;
            _mapper = mapper;
            _repository = _Repository;
        }

      

       
        public async Task<IEnumerable<ResultProductDto>> GetAllProductAsync()
        {
            var product= await _repository.GetAllWithNotesAsync();
            var dto=_mapper.Map<IEnumerable<ResultProductDto>>(product);
            return dto;
        }

        public async Task<GetByIdProductDto> GetByIdProductAsync(int id)
        {
            var product= await _productRepository.GetByIdAsync(id);
            var map = _mapper.Map<GetByIdProductDto>(product);
            return map;
        }

        public async Task CreateProductAsync(CreateProductDto productdto)
        {
            await _productRepository.AddAsync(new Product
            {
                Name = productdto.Name,
                Description = productdto.Description,
                //Size = productdto.Size,
                //CurrentPrice = productdto.CurrentPrice,
                //OriginalPrice = productdto.OriginalPrice,
                ImageUrl = productdto.ImageUrl,
                IsNew = productdto.IsNew,
                IsBestseller = productdto.IsBestseller,
                Disclaimer = productdto.Disclaimer,
                BrandId = productdto.BrandId,
                FamilyId = productdto.FamilyId,
                CategoryId = productdto.CategoryId,
                ProductVariants = productdto.ProductVariants.Select(n => new ProductVariant {
                    Size = n.Size,
                    CurrentPrice = n.CurrentPrice,
                    OriginalPrice=n.OriginalPrice
                }).ToList(),
                ProductNotes = productdto.ProductNotes.Select(n => new ProductNote
                {
                    FragranceNoteId = n.FragranceNoteId,
                    Type = n.Type,
                }).ToList()
                
            });
           
            await _productRepository.SaveChangesAsync();
            
        }

        public async Task UpdateProductAsync(UpdateProductDto model)
        {
            //var products = await _productRepository.GetByIdAsync(model.Id);
            //products.Name = model.Name;
            //products.ImageUrl = model.ImageUrl;
            //products.Price = model.Price;
            //products.Size = model.Size;
            //products.Brand = model.Brand;
            //products.TopNotes = model.TopNotes;
            //products.MiddleNotes = model.MiddleNotes;
            //products.BaseNotes = model.BaseNotes;
            //products.StockQuantity = model.StockQuantity;
            //products.Discount = model.Discount;
            //products.Description = model.Description;
            

            //wait _productRepository.UpdateAsync(products);
            await _productRepository.SaveChangesAsync();
        }

        public async Task DeleteProductAsync(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product != null)
            {
                await _productRepository.DeleteAsync(product);
                await _productRepository.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Product>> FindAsync(Expression<Func<Product, bool>> predicate)
        {
            return await _productRepository.FindAsync(predicate);
        }

        public async Task<List<ResultProductDto>> GetProductByCategory(int? categoryId) 
        {
            var value =await _repository.GetProductByCategory(categoryId);
            var map = _mapper.Map<List<ResultProductDto>>(value);
            return map;
        }
        public async Task<List<ResultProductDto>> GetProductByPriceFilter(int? minPrice, int? maxPrice) 
        {
            var values = await _repository.GetProductByPriceFilter(minPrice, maxPrice);
            var map= _mapper.Map<List<ResultProductDto>>(values);
            return map;
        }
        public async Task<List<ResultProductDto>> GetProductBySearch(string search) 
        {
            var values = await _repository.GetProductBySearch(search);
            var map = _mapper.Map<List<ResultProductDto>>(values);
            return map;
        }
        public async Task<List<ResultProductDto>> GetProductTake(int count) 
        {
            var values=await _productRepository.GetTakeAsync(count);
            var map = _mapper.Map<List<ResultProductDto>>(values);
            return map;
        }

        public async Task<List<ResultProductDto>> GetAllWithNotesAsync()
        {
            var values= await _repository.GetAllWithNotesAsync();
            var map =_mapper.Map<List<ResultProductDto>>(values);
            return map;
        }

        public async Task<List<ResultProductDto>> GetDailyRotatedProductsAsync(int categoryId, int count)
        {
            var values = await _repository.GetDailyRotatedProductsAsync(categoryId,count);
            var map = _mapper.Map<List<ResultProductDto>>(values);
            return map;
        }

      
        public async Task<List<ResultProductDto>> GetBestsellerProductsAsync(int count)
        {
            var values = await _repository.GetBestsellerProductsAsync(count);
            var map = _mapper.Map<List<ResultProductDto>>(values);
            return map;
        }

        public async Task<List<ResultProductDto>> GetNewProductsAsync()
        {
            var values= await _repository.GetNewProductsAsync();
            var map = _mapper.Map<List<ResultProductDto>>(values);
            return map;
        }

        public async Task<PaginatedResult<ResultProductDto>> GetPagedProductsAsync(List<int> categoryIds, List<int> brandId, List<int> fragranceFamilyId, List<int> fragranceNoteId, int page, int pageSize, int? minPrice, int? maxPrice)
        {
            var values = await _repository.GetPagedProductsAsync(categoryIds,brandId,fragranceFamilyId,fragranceNoteId,page,pageSize,minPrice,maxPrice);
            var map = _mapper.Map<PaginatedResult<ResultProductDto>>(values);
            return map;
        }

        public async Task<PaginatedResult<ResultProductDto>> GetPagedProductsByCategoryAsync(int categoryId, int page, int pageSize)
        {

            var valules = await _repository.GetPagedProductsByCategoryAsync(categoryId, page, pageSize);
            return _mapper.Map<PaginatedResult<ResultProductDto>>(valules);
        }

        public async Task<GetByIdProductDto> GetProductByIdWithNotesAsync(int id)
        {
            var value= await _repository.GetProductByIdWithNotes(id);
            return _mapper.Map<GetByIdProductDto>(value);

        }

        public async Task<ResultProductVariantDto> GetByIdProductVariantAsync(int id)
        {
           var variant = await _productRepository.GetByIdAsync(id);
            if (variant == null)
                throw new Exception("Variant tapılmadı");
            var map= _mapper.Map<ResultProductVariantDto>(variant);
            return map;

        }
    }


}
