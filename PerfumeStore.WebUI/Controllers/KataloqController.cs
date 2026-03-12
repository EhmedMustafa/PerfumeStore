using Microsoft.AspNetCore.Mvc;
using PerfumeStore.Application.Dtos.ProductDtos;
using PerfumeStore.Application.Interfaces.IProductVariant;
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
        private readonly IProductVariantRepository _productVariantRepository;

        public KataloqController(IProductService productService, IFragranceFamilyService fragranceFamily, IBrandService brandService, IFragranceNoteService fragranceNoteService, IProductVariantRepository productVariantRepository)
        {
            _productService = productService;
            _fragranceFamily = fragranceFamily;
            _brandService = brandService;
            _fragranceNoteService = fragranceNoteService;
            _productVariantRepository = productVariantRepository;
        }

        public async Task<IActionResult> Index(List<int> categoryId , List<int> brandId, List<int> fragranceFamilyId,List<int> fragranceNoteId ,int? minPrice,int? maxPrice, int page = 1)
        {
            int min = minPrice ?? 0;
            int max = maxPrice ?? 1000;

            int pageSize = 9;
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
               fragranceNoteId.Count > 0 ? fragranceNoteId:null,
               page,
               pageSize,
               min,
               max
           );

            var categoryCounts = await _productService.GetCategoryCountsAsync();


            ViewBag.SelectedCategories = categoryId;
            ViewBag.SelectedBrand = brandId;
            ViewBag.SelectedFamily = fragranceFamilyId;
            ViewBag.SelectedNote = fragranceNoteId;
            ViewBag.MinPrice = min;
            ViewBag.MaxPrice = max;
            ViewBag.CategoryCounts = categoryCounts;



            return View(pagedResult);
        }

        [HttpGet("Detail/{id}")]
        public async Task<IActionResult> Detail(int id) 
        {
            var value= await _productService.GetProductByIdWithNotesAsync(id);
            return View(value);
        }


        [HttpGet("/ProductVariants/GetByProductId/{productId}")]
        public async Task<IActionResult> GetByProductId(int productId)
        {
            var variants = await _productVariantRepository.GetVariantsByProductIdAsync(productId);
            var result = variants.Select(v => new
            {
                id = v.Id,
                size = v.Size,
                originalPrice = v.OriginalPrice,
                currentPrice = v.CurrentPrice,
                stockQuantity = 100 // Müvəqqəti olaraq 100 təyin edirəm, sonra database-dən gətirə bilərsiniz
            }).ToList();

            return Ok(result);
        }

    }
}
