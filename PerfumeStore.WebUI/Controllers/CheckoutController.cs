using Microsoft.AspNetCore.Mvc;
using PerfumeStore.Application.Services.CartServices;

namespace PerfumeStore.WebUI.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly ICartService _cartService;

        public CheckoutController(ICartService cartService)
        {
            _cartService = cartService;
        }

        public async Task<IActionResult> Index()
        {
            var userId = 1;
            var values = await _cartService.GetCartByUserIdWithItemsAsync(userId);
            return View(values);
        }
    }
}
