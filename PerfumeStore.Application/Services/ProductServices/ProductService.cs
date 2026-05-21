using AutoMapper;
using PerfumeStore.Application.Dtos.ProductDtos;
using PerfumeStore.Application.Dtos.ProductVariantDtos;
using PerfumeStore.Application.Interfaces;
using PerfumeStore.Application.Interfaces.IProductRepository;
using PerfumeStore.Application.Interfaces.IProductVariant;
using PerfumeStore.Application.Services.ProductServices;
using PerfumeStore.Domain.Entities;
using System;
using System.Linq.Expressions;
using System.Text.Json;

namespace PerfumeStore.Infrastructure.Services
{
    public class ProductService : IProductService
    {
        private readonly IGenericRepository<Product> _productRepository;
        private readonly IMapper _mapper;
        private readonly IProductRepository _repository;
        private readonly IProductVariantRepository _productVariant;

        public ProductService(IGenericRepository<Product> productRepository, IMapper mapper, IProductRepository _Repository, IProductVariantRepository productVariant)
        {
            _productRepository = productRepository;
            _mapper = mapper;
            _repository = _Repository;
            _productVariant = productVariant;
        }




        public async Task<IEnumerable<ResultProductDto>> GetAllProductAsync()
        {
            var product= await _repository.GetAllWithNotesAsync();
            var dto=_mapper.Map<IEnumerable<ResultProductDto>>(product);
            return dto;
        }

        public async Task<GetByIdProductDto> GetByIdProductAsync(int id)
        {
            // Variant və notlarla birgə yüklə — admin edit form-u və məhsul detal səhifəsi üçün
            var product = await _repository.GetProductByIdWithNotes(id);
            var map = _mapper.Map<GetByIdProductDto>(product);
            return map;
        }

        public async Task CreateProductAsync(CreateProductDto productdto)
        {
            await _productRepository.AddAsync(new Product
            {
                Name = productdto.Name,
                Description = productdto.Description,
                ImageUrl = productdto.ImageUrl,
                IsNew = productdto.IsNew,
                IsBestseller = productdto.IsBestseller,
                Disclaimer = productdto.Disclaimer,
                Season = productdto.Season,
                Occasion = productdto.Occasion,
                IsAccessory = productdto.IsAccessory,
                IsMonthlyFeatured = productdto.IsMonthlyFeatured,
                GalleryImagesJson = SerializeGallery(productdto.GalleryImages),
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
            // Variant və notlarla birgə yüklə — köhnələri silib təzələri yazacağıq
            var product = await _repository.GetProductByIdWithNotes(model.ProductId);
            if (product == null) throw new InvalidOperationException("Məhsul tapılmadı");

            // Skalar sahələr
            product.Name = model.Name;
            product.Description = model.Description;
            product.ImageUrl = model.ImageUrl;
            product.IsNew = model.IsNew;
            product.IsBestseller = model.IsBestseller;
            product.Disclaimer = model.Disclaimer;
            product.Season = model.Season;
            product.Occasion = model.Occasion;
            product.IsAccessory = model.IsAccessory;
            product.IsMonthlyFeatured = model.IsMonthlyFeatured;
            product.GalleryImagesJson = SerializeGallery(model.GalleryImages);
            product.BrandId = model.BrandId;
            product.FamilyId = model.FamilyId;
            product.CategoryId = model.CategoryId;

            // Variantları yenilə — köhnələri sil, təzələri əlavə et
            product.ProductVariants = (model.ProductVariants ?? new List<ProductVariantCreateDto>())
                .Select(v => new ProductVariant
                {
                    Size = v.Size,
                    CurrentPrice = v.CurrentPrice,
                    OriginalPrice = v.OriginalPrice
                }).ToList();

            // Notları yenilə (UpdateProductDto-da ProductNote tipindədir)
            product.ProductNotes = (model.ProductNotes ?? new List<ProductNote>())
                .Select(n => new ProductNote
                {
                    FragranceNoteId = n.FragranceNoteId,
                    Type = n.Type
                }).ToList();

            await _productRepository.SaveChangesAsync();
        }

        public async Task DeleteProductAsync(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product != null)
            {
                await _repository.DeleteProduct(id);
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
        public async Task<List<ResultProductDto>> GetProductBySearchAsync(string search) 
        {
            var products = await _repository.GetProductBySearch(search);

            return products.Select(p => new ResultProductDto
            {
                ProductId = p.ProductId,
                Name = p.Name,
                ImageUrl = p.ImageUrl,
                ProductVariants = p.ProductVariants?
               .Select(a => new ProductVariantCreateDto
               {
                   CurrentPrice = a.CurrentPrice

               }).ToList() ?? new List<ProductVariantCreateDto>()
            }).ToList();
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
           var variant = await _productVariant.GetVariantWithDetailsByIdAsync(id);
            if (variant == null)
                throw new Exception("Variant tapılmadı");
            var map= _mapper.Map<ResultProductVariantDto>(variant);
            return map;

        }

        public async Task<ResultProductDto> GetByIdProductforWishlist(int id)
        {
            var product= await _repository.GetByIdProductforWishlist(id);
            var map = _mapper.Map<ResultProductDto>(product);
            return map;
        }

        public async Task<Dictionary<int, int>> GetCategoryCountsAsync()
        {
            return await _repository.GetCategoryCount();
        }

        // Gallery URL siyahısını JSON-a çevir (DB-də saxlanır)
        private static string SerializeGallery(List<string> images)
        {
            if (images == null || images.Count == 0) return null;
            var clean = images.Where(u => !string.IsNullOrWhiteSpace(u)).Select(u => u.Trim()).ToList();
            if (clean.Count == 0) return null;
            return JsonSerializer.Serialize(clean);
        }
    }


}
