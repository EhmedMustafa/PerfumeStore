using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PerfumeStore.Application.Dtos.ProductDtos;
using PerfumeStore.Application.Services.ProductServices;
using PerfumeStore.Domain.Entities;

namespace PerfumeStore.WebUI.Controllers
{
    public class WishlistController : Controller
    {
        private static List<ResultProductDto> wistlist = new List<ResultProductDto>();
        private readonly IProductService _productService;
        private readonly IMapper _mapper;

        public WishlistController(IProductService productService, IMapper mapper)
        {
            _productService = productService;
            _mapper = mapper;
        }

        public async Task <IActionResult> Index()
        {
            var dtoList =  _mapper.Map<List<ResultProductDto>>(wistlist);
            return View(dtoList);
        }

        public async Task<IActionResult> Add(int id) 
        {
            
            var product = await _productService.GetByIdProductAsync(id);
            var map = _mapper.Map<ResultProductDto>(product);

            if (map.ProductVariants != null && map.ProductVariants.Any())
            {
                map.ProductVariants.FirstOrDefault().CurrentPrice = map.ProductVariants.First().CurrentPrice;
            }

            if (!wistlist.Any(x => x.ProductId == id))
                wistlist.Add(map);
            return RedirectToAction("Index");
        }


    }
}
