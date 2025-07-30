using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PerfumeStore.Application.Services.ProductServices;

namespace PerfumeStore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductVariantController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductVariantController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet("{Id}")]

        public async Task<IActionResult> GetById(int Id) 
        {
            var values = await _productService.GetByIdProductVariantAsync(Id);
            return Ok(values);

        }
    }
}
