    using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using PerfumeStore.Application.Dtos.FragrancefamilyDtos;
using PerfumeStore.Application.Interfaces;
using PerfumeStore.Domain.Entities;

namespace PerfumeStore.Application.Services.FragranceFamilyService
{
    internal class FragranceFamilyService : IFragranceFamilyService
    {

        private readonly IGenericRepository<FragranceFamily> _genericRepository;
        private readonly IMapper _mapper;

        public FragranceFamilyService(IGenericRepository<FragranceFamily> genericRepository,IMapper mapper)
        {
            _genericRepository = genericRepository;
            _mapper = mapper;
        }

        public async Task AddFragranceFamilyAsync(CreateFragranceFamilyDto createFragranceFamilyDto)
        {
            var values = _genericRepository.AddAsync (new FragranceFamily
            {
                Name=createFragranceFamilyDto.Name,
            });
            await _genericRepository.SaveChangesAsync();
        }

        public async Task<IEnumerable<ResultFragranceFamilyDto>> GetAllFragranceFamilyAsync()
        {
            var values= await _genericRepository.GetAllAsync();
            var map = _mapper.Map<IEnumerable<ResultFragranceFamilyDto>>(values);
            return map;
        }

        public async Task<GetByIdFragranceFamilyDto> GetByIdFragranceFamilyAsync(int id)
        {
            var value= await _genericRepository.GetByIdAsync(id);
            var map = _mapper.Map<GetByIdFragranceFamilyDto>(value);
            return map;
        }

        public async Task UpdateFragranceFamilyAsync(UpdateFragranceFamilyDto updateFragranceFamilyDto)
        {
            var values = await _genericRepository.GetByIdAsync(updateFragranceFamilyDto.Id);
            values.Name=updateFragranceFamilyDto.Name;
            await _genericRepository.UpdateAsync(values);
            await _genericRepository.SaveChangesAsync();
        }
        public async Task DeleteFragranceFamilyAsync(int id)
        {
            var value= await _genericRepository.GetByIdAsync(id);
            await _genericRepository.DeleteAsync(value);
            await _genericRepository.SaveChangesAsync();
        }

       

        

        
    }
}
