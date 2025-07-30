using System.Net.WebSockets;
using Microsoft.AspNetCore.Mvc;
using PerfumeStore.Application.Dtos.CartItemDtos;
using PerfumeStore.Application.Services.CartItemItemServices;
using PerfumeStore.Application.Services.CartServices;
using PerfumeStore.Application.Services.ProductServices;

namespace PerfumeStore.WebUI.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartItemService _cartItemService;
        private readonly ICartService _cartService;
        private readonly IProductService _productService;
        public CartController(ICartItemService cartItemService, ICartService cartService, IProductService productService)
        {
            _cartItemService = cartItemService;
            _cartService = cartService;
            _productService = productService;
        }

        public async Task<IActionResult> Index()
        {
            var userId = 1;
            var values = await _cartService.GetCartByUserIdWithItemsAsync(userId);
       
            return View(values);
        }
      
        public async Task<IActionResult> RefreshCartDropdown()
        {
            var userId = 1;
            var cart = await _cartService.GetCartByUserIdWithItemsAsync(userId);
            return ViewComponent("CartMenu", new { userId = userId }); // ViewComponent çağır
        }


        [HttpPost]
        public async Task<IActionResult> AddToCartItem([FromBody] CreateCartItemDto model)
        {

            try
            {
                int cartId = 1; // ya userId-dən səbəti tap, ya statik, ya da sessiondan götür

                await _cartItemService.AddCartItemAsync(cartId, model);


                return Json(new { success = true, message = "Məhsul səbətə əlavə olundu." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }

           

        }

        [HttpPost]
        public async Task<IActionResult> DeleteCartItem(int id)
        {
            try
            {
                var result = await _cartItemService.DeleteCartItemAsync(id);
                
                if (result)
                {
                    TempData["Success"] = "Məhsul səbətdən silindi.";
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<JsonResult> UpdateCartItemAjax([FromBody] UpdateCartItemDto dto)
        {
            try
            {
                var result = await _cartItemService.UpdateCartItemAsync(dto);
                return Json(new { success = result });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
    }
}
