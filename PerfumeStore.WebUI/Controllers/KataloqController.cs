using Microsoft.AspNetCore.Mvc;
using PerfumeStore.Application.Dtos.ProductDtos;
using PerfumeStore.Application.Services.FragranceFamilyService;
using PerfumeStore.Application.Services.ProductServices;
using PerfumeStore.Domain.Entities;

namespace PerfumeStore.WebUI.Controllers
{
    public class KataloqController : Controller
    {
        private readonly IProductService _productService;
        private readonly IFragranceFamilyService _fragranceFamily;

        public KataloqController(IProductService productService, IFragranceFamilyService fragranceFamily)
        {
            _productService = productService;
            _fragranceFamily = fragranceFamily;
        }

        public async Task<IActionResult> Index(List<int> categoryId , int? brandId, int? fragranceFamilyId, int page = 1)
        {
            int pageSize = 9;
            var allFamilies = await _fragranceFamily.GetAllFragranceFamilyAsync();

            ViewBag.AllFamilies = allFamilies;
            //var products = await _productService.GetAllProductAsync();


            var pagedResult = await _productService.GetPagedProductsAsync(
               categoryId.Count > 0 ? categoryId : null,
               brandId,
               fragranceFamilyId,
               page,
               pageSize
           );
            ViewBag.SelectedCategories = categoryId;
            ViewBag.SelectedBrand = brandId;
            ViewBag.SelectedFamily = fragranceFamilyId;


          //  var pagedResult = await _productService.GetPagedProductsAsync(categoryId, brandId, fragranceFamilyId, page, pageSize);

            return View(pagedResult);
        }
    }
}
