using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PerfumeStore.Application.Dtos.CartDtos;
using PerfumeStore.Application.Services.CartServices;

namespace PerfumeStore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCart()
        {
            var values= await _cartService.GetAllCartAsync();
            return Ok(values);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCart([FromBody] CreateCartDto model) 
        {
            await _cartService.AddCartAsync(model);
            return Ok("Səbət Əlavə olundu");
        }

        [HttpGet("{Id}")]

        public async Task<IActionResult> GetByIdCart(int Id)
        {
            var value= await _cartService.GetByIdCartAsync(Id);
            return Ok(value);
        }

        [HttpDelete]

        public async Task<IActionResult> DeleteCart(int Id)
        {
            await _cartService.DeleteCartAsync(Id);
            return Ok("Sebet silindi");
        }
    }
}
