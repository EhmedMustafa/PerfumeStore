using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PerfumeStore.Application.Dtos.ProductDtos;
using PerfumeStore.Application.Interfaces;
using PerfumeStore.Application.Services.ProductServices;
using PerfumeStore.Domain.Entities;
using Stripe.Climate;
using Product = PerfumeStore.Domain.Entities.Product;

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
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateProductDto product) 
        {
            await _productService.CreateProductAsync(product);
            return Ok("Mehsul yüklendi");
        }

        [HttpPut]
        public async Task<IActionResult> Update(UpdateProductDto product)
        {
          await _productService.UpdateProductAsync(product);
            return Ok("Mehsul yeniləndi");
        }

        [HttpDelete]
        public async Task<IActionResult> Delete (int Id) 
        {
            await _productService.DeleteProductAsync(Id);
            return Ok("Mehsul Silindi");
        }
    }
}
