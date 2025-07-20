using Microsoft.AspNetCore.Mvc;
using PerfumeStore.Application.Dtos.ProductDtos;
using PerfumeStore.Application.Services.BrandServices;
using PerfumeStore.Application.Services.FragranceFamilyService;
using PerfumeStore.Application.Services.FragranceNoteServices;
using PerfumeStore.Application.Services.ProductServices;
using PerfumeStore.Domain.Entities;

namespace PerfumeStore.WebUI.Controllers
{
    public class KataloqController : Controller
    {
        private readonly IProductService _productService;
        private readonly IFragranceFamilyService _fragranceFamily;
        private readonly IBrandService _brandService;
        private readonly IFragranceNoteService _fragranceNoteService;

        public KataloqController(IProductService productService, IFragranceFamilyService fragranceFamily, IBrandService brandService, IFragranceNoteService fragranceNoteService)
        {
            _productService = productService;
            _fragranceFamily = fragranceFamily;
            _brandService = brandService;
            _fragranceNoteService = fragranceNoteService;
        }

        public async Task<IActionResult> Index(List<int> categoryId , List<int> brandId, List<int> fragranceFamilyId,List<int> fragranceNoteId ,int page = 1)
        {
            int pageSize = 12;
            var allFamilies = await _fragranceFamily.GetAllFragranceFamilyAsync();
            var allbrands= await _brandService.GetAllBrandAsync();
            var notes= await _fragranceNoteService.GetAllFragranceNoteAsync();

            ViewBag.AllFamilies = allFamilies;
            ViewBag.Allbrands = allbrands;
            ViewBag.Notes = notes;
            //var products = await _productService.GetAllProductAsync();


            var pagedResult = await _productService.GetPagedProductsAsync(
               categoryId.Count > 0 ? categoryId : null,
               brandId,
               fragranceFamilyId,
               fragranceNoteId,
               page,
               pageSize
           );
            ViewBag.SelectedCategories = categoryId;
            ViewBag.SelectedBrand = brandId;
            ViewBag.SelectedFamily = fragranceFamilyId;
            ViewBag.SelectedNote = fragranceNoteId;


          //  var pagedResult = await _productService.GetPagedProductsAsync(categoryId, brandId, fragranceFamilyId, page, pageSize);

            return View(pagedResult);
        }
    }
}
