using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using PerfumeStore.Application.Dtos.BrandDtos;
using PerfumeStore.Application.Dtos.ProductDtos;
using PerfumeStore.Application.Interfaces;
using PerfumeStore.Application.Interfaces.IBrandRepository;
using PerfumeStore.Domain.Entities;

namespace PerfumeStore.Application.Services.BrandServices
{
    public class BrandService : IBrandService
    {
        private readonly IGenericRepository<Brand> _genericRepository;
        private readonly IBrandRepository _brandRepository;
        private readonly IMapper _mapper;

        public BrandService(IGenericRepository<Brand> genericRepository, IMapper mapper, IBrandRepository brandRepository)
        {
            _genericRepository = genericRepository;
            _mapper = mapper;
            _brandRepository = brandRepository;
        }

        public async Task AddBrandAsync(CreateBrandDto createBrandDto)
        {
            var valeus = _genericRepository.AddAsync(new Brand
            {
                Name = createBrandDto.Name,
                LogoUrl = createBrandDto.LogoUrl,
            });
            await _genericRepository.SaveChangesAsync();
        }
        public async Task<IEnumerable<ResultBrandDto>> GetAllBrandAsync()
        {
            var values = await _genericRepository.GetAllAsync();
            var map = _mapper.Map<IEnumerable<ResultBrandDto>>(values);
            return map;
        }

        public async Task<GetByIdBrandDto> GetByIdBrandAsync(int id)
        {
            var values= await _genericRepository.GetByIdAsync(id);
            var map= _mapper.Map<GetByIdBrandDto>(values);
            return map;
        }
        public async Task UpdateBrandAsync(UpdateBrandDto updateBrandDto)
        {
            var values = await _genericRepository.GetByIdAsync(updateBrandDto.Id);
            values.Name = updateBrandDto.Name;
            values.LogoUrl = updateBrandDto.LogoUrl;
            await _genericRepository.UpdateAsync(values);
            await _genericRepository.SaveChangesAsync();
        }
        public async Task DeleteBrandAsync(int id)
        {
            var values = await _genericRepository.GetByIdAsync(id);
            await _genericRepository.DeleteAsync(values);
            await _genericRepository.SaveChangesAsync();
        }

        public async Task<ResultBrandDto> GetBrandDetailsWithProductsAsync(int brandId)
        {
            var brand = await _brandRepository.GetBrandWithProductsByIdAsync(brandId);

            if (brand == null)
                return null;

            return new ResultBrandDto
            {
                Id = brand.Id,
                Name = brand.Name,
                Tagline = brand.Tagline,
                Description = brand.Description,
                LogoUrl = brand.LogoUrl,
                products = brand.products.Select(p => new ResultProductDto
                {
                    ProductId = p.ProductId,
                    Name = p.Name,
                    Description = p.Description,
                    Size = p.Size,
                    CurrentPrice = p.CurrentPrice,
                    OriginalPrice = p.OriginalPrice,
                    ImageUrl = p.ImageUrl,
                    IsNew = p.IsNew,
                    IsBestseller = p.IsBestseller,
                    Disclaimer = p.Disclaimer,
                    BrandId = p.BrandId
                }).ToList()
            };
        }
    }
}
