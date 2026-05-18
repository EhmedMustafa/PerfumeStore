using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PerfumeStore.Application.Dtos.ProductDtos;
using PerfumeStore.Application.Interfaces;
using PerfumeStore.Application.Services.ProductServices;
using PerfumeStore.Domain.Entities;

namespace PerfumeStore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
         private readonly IProductService _productService;
         private readonly IMapper _mapper;

        public ProductController(IProductService productService,IMapper mapper)
        {
            _productService = productService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var product= await _productService.GetAllWithNotesAsync();
            return Ok(product);
        }

        // GET /api/Product/paged?page=1&pageSize=24&minPrice=&maxPrice=&categoryIds=1&brandIds=2,3
        // Frontend pagination üçün — bütün məhsulları yox, 24-24 yükləmək
        [HttpGet("paged")]
        public async Task<IActionResult> GetPaged(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 24,
            [FromQuery] int? minPrice = null,
            [FromQuery] int? maxPrice = null,
            [FromQuery] string categoryIds = null,
            [FromQuery] string brandIds = null,
            [FromQuery] string familyIds = null,
            [FromQuery] string noteIds = null)
        {
            if (page < 1) page = 1;
            if (pageSize < 1 || pageSize > 100) pageSize = 24;

            List<int> Parse(string s) => string.IsNullOrWhiteSpace(s)
                ? new List<int>()
                : s.Split(',', StringSplitOptions.RemoveEmptyEntries)
                   .Select(x => int.TryParse(x.Trim(), out var n) ? n : 0)
                   .Where(n => n > 0).ToList();

            var result = await _productService.GetPagedProductsAsync(
                Parse(categoryIds),
                Parse(brandIds),
                Parse(familyIds),
                Parse(noteIds),
                page,
                pageSize,
                minPrice,
                maxPrice);

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetbyId (int id) 
        {
            var product= await _productService.GetByIdProductAsync(id);
            if (product==null)
                return NotFound();
            return Ok(product);
        }

        [HttpGet("daily-rotated")]
        public async Task<IActionResult> GetDailyRotatedProducts([FromQuery] int categoryId, [FromQuery] int count = 6) 
        {
            var values= await _productService.GetDailyRotatedProductsAsync(categoryId, count);
            return Ok(values);
        }

        [HttpGet("bestsellers")]
        public async Task<IActionResult> GetBestsellerProducts([FromQuery]int count=12) 
        {
            var values= await _productService.GetBestsellerProductsAsync(count);
            return Ok(values);
        }

        [HttpGet("News")]
        public async Task<IActionResult> GetNewProducts() 
        {
            var values = await _productService.GetNewProductsAsync();
            return Ok(values);
        }
        [HttpGet("categoryId")]
        public async Task<IActionResult> GetProductByCategoryId(int categoryId) 
        {
            var value= await _productService.GetProductByCategory(categoryId);
            if (value == null)
                return NotFound();
            return Ok(value);
        }
        [HttpGet("Search")]
        public async Task<IActionResult> SearchProducts([FromQuery] string q)
        {
            var value= await _productService.GetProductBySearchAsync(q);
            if (value==null)
            {
                return NotFound("Tapilmadi");
            }

            return Ok(value);
        }

        [HttpGet("WithProductNotes")]
        public async Task<IActionResult> GetProductIdByNotes(int id) 
        {
            var value= await _productService.GetProductByIdWithNotesAsync(id);
            return Ok(value);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] CreateProductDto product)
        {
            await _productService.CreateProductAsync(product);
            return Ok("Mehsul yüklendi");
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(UpdateProductDto product)
        {
          await _productService.UpdateProductAsync(product);
            return Ok("Mehsul yeniləndi");
        }

        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete (int Id)
        {
            await _productService.DeleteProductAsync(Id);
            return Ok("Mehsul Silindi");
        }
    }
}
