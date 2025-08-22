using Microsoft.AspNetCore.Mvc;
using PerfumeStore.Application.Services.CartItemItemServices;
using PerfumeStore.Application.Services.CartServices;

namespace PerfumeStore.WebUI.Views.Shared.Components.ViewComponents
{
    public class CartMenuViewComponent : ViewComponent
    {
        private readonly ICartService _cartService;

        public CartMenuViewComponent(ICartService cartService)
        {
            _cartService = cartService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var userId = 1;
            var values = await _cartService.GetCartByUserIdWithItemsAsync(userId);
            int totalQuantity = values?.CartItems?.Sum(x => x.Quantity) ?? 0;

            ViewBag.TotalQuantity = totalQuantity;
            return View(values);
        }
             
    }
}
