using Microsoft.AspNetCore.Mvc;
using PerfumeStore.Application.Services.BrandServices;
using PerfumeStore.Application.Services.ProductServices;
using PerfumeStore.Domain.Entities;

namespace PerfumeStore.WebUI.Controllers
{
    [Route("Brand")]
    public class BrandController : Controller
    {
        private readonly IBrandService _brandService;
        
        public BrandController(IBrandService brandService)
        {
            _brandService = brandService;
        }
        [HttpGet("{brandId}")]
        public async Task<IActionResult>  Index(int brandId)
        {
           var values= await _brandService.GetBrandDetailsWithProductsAsync(brandId);
            if (values == null)
            {
                return NotFound();
            }

            return View(values);
        }
    }
}
