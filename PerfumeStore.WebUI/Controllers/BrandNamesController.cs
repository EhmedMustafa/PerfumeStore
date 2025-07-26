using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using PerfumeStore.Application.Services.BrandServices;
using RouteAttribute = Microsoft.AspNetCore.Components.RouteAttribute;

namespace PerfumeStore.WebUI.Controllers
{
    
    public class BrandNamesController : Controller
    {
        private readonly IBrandService _brandService;

        public BrandNamesController(IBrandService brandService)
        {
            _brandService = brandService;
        }
        [HttpGet("/BrandNames")] // <-- Bunu əlavə et
        public async Task<IActionResult> Index()
        {
           var values = await _brandService.GetGroupedBrandsAsync();
            return View(values);
        }
        
    }
}
