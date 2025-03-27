using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using PerfumeStore.Application.Dtos.CategoryDtos;
using PerfumeStore.Application.Interfaces;
using PerfumeStore.Application.Services;
using PerfumeStore.Domain.Entities;

namespace PerfumeStore.Infrastructure.Services
{
    public class CategoryService:ICategoryService
    {
        private readonly IGenericRepository<Category> _categoryRepository;
        private readonly IMapper _mapper;

        public CategoryService(IGenericRepository<Category> categoryRepository,IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ResultCategoryDto>> GetAllCategoryAsync()
        {
            var getall= await _categoryRepository.GetAllAsync();
            var map = _mapper.Map<IEnumerable<ResultCategoryDto>>(getall);
            return map;
        }

        public async Task<GetByIdGategoryDto> GetByIdCategoryAsync(int id)
        {
            var values= await _categoryRepository.GetByIdAsync(id);
            var map= _mapper.Map<GetByIdGategoryDto>(values);
            return map;

        }

        public async Task AddCategoryAsync(CreateCategoryDto createCategoryDto )
        {
            await _categoryRepository.AddAsync(createCategoryDto);
            await _categoryRepository.SaveChangesAsync();
        }

        public async Task UpdateCategoryAsync(UpdateCategoryDto updateCategoryDto)
        {
            await _categoryRepository.UpdateAsync(updateCategoryDto);
            await _categoryRepository.SaveChangesAsync();
        }

        public async Task DeleteCategoryAsync(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category != null)
            {
                await _categoryRepository.DeleteAsync(category);
                await _categoryRepository.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Category>> FindAsync(Expression<Func<Category, bool>> predicate)
        {
            return await _categoryRepository.FindAsync(predicate);
        }
    }
}
