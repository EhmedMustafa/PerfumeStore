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

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() 
        {
            var product= await _productService.GetAllProductAsync();
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
