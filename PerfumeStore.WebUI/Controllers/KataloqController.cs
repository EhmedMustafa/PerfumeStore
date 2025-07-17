using Microsoft.AspNetCore.Mvc;
using PerfumeStore.Application.Dtos.ProductDtos;
using PerfumeStore.Application.Services.ProductServices;
using PerfumeStore.Domain.Entities;

namespace PerfumeStore.WebUI.Controllers
{
    public class KataloqController : Controller
    {
        private readonly IProductService _productService;

        public KataloqController(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<IActionResult> Index(int? categoryId,int? brandId, int? fragranceFamilyId , int page = 1)
        {

            int pageSize = 9;

            ViewBag.SelectedCategory = categoryId;
            ViewBag.SelectedBrand = brandId;

            // PaginatedResult<ResultProductDto> pagedResult;


            var pagedResult = await _productService.GetPagedProductsAsync(categoryId,brandId,fragranceFamilyId,page,pageSize);
            

            return View(pagedResult);

        }
    }
}
