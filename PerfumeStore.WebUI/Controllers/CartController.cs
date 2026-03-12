using System.Net.WebSockets;
using Microsoft.AspNetCore.Mvc;
using PerfumeStore.Application.Dtos.CartDtos;
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
        private const string CartSessionKey = "CartId";

        public CartController(ICartItemService cartItemService, ICartService cartService, IProductService productService)
        {
            _cartItemService = cartItemService;
            _cartService = cartService;
            _productService = productService;
        }

        /// <summary>
        /// Session-dan CartId götürür, yoxdursa yeni səbət yaradır
        /// </summary>
        private async Task<int> GetOrCreateCartIdAsync()
        {
            var cartId = HttpContext.Session.GetInt32(CartSessionKey);
            if (cartId.HasValue)
                return cartId.Value;

            // Yeni səbət yarat
            var newCartId = await _cartService.CreateCartAsync();
            HttpContext.Session.SetInt32(CartSessionKey, newCartId);
            return newCartId;
        }

        public async Task<IActionResult> Index()
        {
            var cartId = await GetOrCreateCartIdAsync();
            var values = await _cartService.GetCartByUserIdWithItemsAsync(cartId);
       
            return View(values);
        }

        public async Task<IActionResult> RefreshCartDropdown()
        {
            var cartId = await GetOrCreateCartIdAsync();
            var cart = await _cartService.GetCartByUserIdWithItemsAsync(cartId);
            return ViewComponent("CartMenu", new { userId = cartId }); // ViewComponent çağır
        }


        [HttpPost]
        public async Task<IActionResult> AddToCartItem([FromBody] CreateCartItemDto model)
        {

            try
            {
                int cartId = await GetOrCreateCartIdAsync();

                await _cartItemService.AddCartItemAsync(cartId, model);

                return Json(new { success = true, message = "Məhsul səbətə əlavə olundu." });
            }


            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        //[HttpPost]
        //public async Task<IActionResult> AddToCartFromDetails([FromBody] AddToCartRequest model) 
        //{
        //    try
        //    {
        //        await.
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}
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
        public async Task<IActionResult> DeletefromCartMenu(int id)
        {
            try
            {
                var result = await _cartItemService.DeleteCartItemAsync(id);

                if (result)
                {
                    return Json(new { success = true, message = "Məhsul səbətdən silindi." });
                }
                else
                {
                    return Json(new { success = false, message = "Silmək alınmadı." });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }

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
