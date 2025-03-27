using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PerfumeStore.Application.Dtos.CategoryDtos;
using PerfumeStore.Domain.Entities;

namespace PerfumeStore.Application.Services
{
    public interface ICategoryService
    {
        Task<IEnumerable<ResultCategoryDto>> GetAllCategoryAsync();
        Task<GetByIdGategoryDto> GetByIdCategoryAsync(int id);
        Task AddCategoryAsync(CreateCategoryDto createCategoryDto);
        Task UpdateCategoryAsync(UpdateCategoryDto updateCategoryDto);
        Task DeleteCategoryAsync(int id);
    }
}
