using AutoMapper;
using PerfumeStore.Application.Dtos.ProductDtos;
using PerfumeStore.Application.Interfaces;
using PerfumeStore.Application.Interfaces.IProductRepository;
using PerfumeStore.Application.Services;
using PerfumeStore.Domain.Entities;
using PerfumeStore.Infrastructure.Repositories.ProductRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

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

        public async Task<IEnumerable<ResultProductDto>> GetAllAsync()
        {
            var product= await _productRepository.GetAllAsync();
            var dto=_mapper.Map<IEnumerable<ResultProductDto>>(product);
            return dto;
        }

        public async Task<ResultProductDto> GetByIdAsync(int id)
        {
            var product= await _productRepository.GetByIdAsync(id);
            var map = _mapper.Map<ResultProductDto>(product);
            return map;
        }

        public async Task AddAsync(ProductCreateDto productdto)
        {
            await _productRepository.AddAsync(new Product
            {
                Name = productdto.Name,
                ImageUrl = productdto.ImageUrl,
                OriginalPrice = productdto.Price,
                Size = productdto.Size,
                //Brand = productdto.Brand,
                //TopNotes = productdto.TopNotes,
                //MiddleNotes = productdto.MiddleNotes,
                //BaseNotes = productdto.BaseNotes,
                //StockQuantity = productdto.StockQuantity,
                //Discount = productdto.Discount,
                //Description = productdto.Description
            });
           // await _productRepository.AddAsync(product);
            await _productRepository.SaveChangesAsync();
        }

        public async Task UpdateAsync(ProductUpdateDto model)
        {
            var products = await _productRepository.GetByIdAsync(model.Id);
            products.Name = model.Name;
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
            

            await _productRepository.UpdateAsync(products);
            await _productRepository.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
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

        public async Task<List<ResultProductDto>> GetProductByCategory(int categoryId) 
        {
            var values =await _repository.GetProductByCategory(categoryId);
            var map = _mapper.Map<List<ResultProductDto>>(values);
            return map;
        }
        public async Task<List<ResultProductDto>> GetProductByPriceFilter(decimal minprice, decimal maxprice) 
        {
            var values = await _repository.GetProductByPriceFilter(minprice, maxprice);
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
    }


}
