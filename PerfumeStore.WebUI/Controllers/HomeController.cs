using Microsoft.AspNetCore.Mvc;
using PerfumeStore.Application.Dtos.ProductDtos;
using PerfumeStore.Application.Services.ProductServices;
using PerfumeStore.Domain.Entities;
using PerfumeStore.WebUI.Models;
using System.Diagnostics;

namespace PerfumeStore.WebUI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProductService _productService;
        public HomeController(ILogger<HomeController> logger, IProductService productService)
        {
            _logger = logger;
            _productService = productService;
        }

        public async Task<IActionResult> Index(int count=12)
        {
            var newproduct = await _productService.GetNewProductsAsync();
            ViewBag.NewProducts = newproduct;
            var bestsellproduct = await _productService.GetBestsellerProductsAsync(count);
            ViewBag.Bestsellproduct = bestsellproduct;

            // categoryId-l?ri ?z?n? uy?unla?d?r!
            var men = await _productService.GetDailyRotatedProductsAsync(categoryId: 1, count);
            var women = await _productService.GetDailyRotatedProductsAsync(categoryId: 2, count);
            var uni = await _productService.GetDailyRotatedProductsAsync(categoryId: 3, count);

            var model = new Dictionary<string, List<ResultProductDto>>
                {
                    { "Men", men },
                    { "Women", women },
                    { "Unisex", uni }
                };

            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet]

        public async Task<IActionResult> SearchAjax(string search) 
        {
            if (string.IsNullOrWhiteSpace(search))
                return Json(new List<object>());

            var result= await _productService.GetProductBySearchAsync(search);
            return Json(result);
        }
    }
}
