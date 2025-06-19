using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PerfumeStore.Application.Dtos.CategoryDtos;
using PerfumeStore.Application.Interfaces;
using PerfumeStore.Application.Services.CategoryServices;
using PerfumeStore.Domain.Entities;

namespace PerfumeStore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
       
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var categories = await _categoryService.GetAllCategoryAsync();
            return Ok(categories);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var category = await _categoryService.GetByIdCategoryAsync(id);
            if (category == null)
                return NotFound();

            return Ok(category);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCategoryDto category)
        {
            if (category == null)
                return BadRequest();

            await _categoryService.AddCategoryAsync(category);
            return Ok("Kateqoriya elave olundu");
        }

        [HttpPut]
        public async Task<IActionResult> Update(UpdateCategoryDto category)
        {
            await _categoryService.UpdateCategoryAsync(category);
            return Ok ("Kateqoriya yenilendi");
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            await _categoryService.DeleteCategoryAsync(id);
            return Ok("Kateqoriya silindi");
        }
    }
}
