using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using PerfumeStore.Application.Dtos.BrandDtos;
using PerfumeStore.Application.Interfaces;
using PerfumeStore.Domain.Entities;

namespace PerfumeStore.Application.Services.BrandServices
{
    public class BrandService : IBrandService
    {
        private readonly IGenericRepository<Brand> _genericRepository;
        private readonly IMapper _mapper;

        public BrandService(IGenericRepository<Brand> genericRepository,IMapper mapper)
        {
            _genericRepository = genericRepository;
            _mapper = mapper;
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

       

     

       
    }
}
