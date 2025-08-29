using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PerfumeStore.Application.Dtos.ProductDtos;
using PerfumeStore.Application.Services.ProductServices;
using PerfumeStore.Domain.Entities;
using PerfumeStore.WebUI.Models;

namespace PerfumeStore.WebUI.Controllers
{
    public class WishlistController : Controller
    {
        private readonly IProductService _productService;
        private readonly IMapper _mapper;

        public WishlistController(IProductService productService, IMapper mapper)
        {
            _productService = productService;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            var wishlist = HttpContext.Session.GetObjectFromJson<List<ResultProductDto>>("Wishlist") ?? new List<ResultProductDto>();
            return View(wishlist);
        }

        [HttpPost]
        public async Task<IActionResult> AddToSession(int id)
        {
            var wishlist = HttpContext.Session.GetObjectFromJson<List<ResultProductDto>>("Wishlist") ?? new List<ResultProductDto>();

            var exists = wishlist.Any(x => x.ProductId == id);
            bool added;

            if (!exists)
            {
                var product = await _productService.GetByIdProductAsync(id);
                var map = _mapper.Map<ResultProductDto>(product);
                wishlist.Add(map);
                added = true;
            }
            else
            {
                wishlist.RemoveAll(x => x.ProductId == id);
                added = false;
            }

            HttpContext.Session.SetObjectAsJson("Wishlist", wishlist);

            return Json(new { added = added, count = wishlist.Count });
        }
    }

}
