using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PerfumeStore.Application.Dtos.CartItemDtos;
using PerfumeStore.Application.Services.CartItemItemServices;

namespace PerfumeStore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartItemController : ControllerBase
    {
        private readonly ICartItemService _cartItemService;

        public CartItemController(ICartItemService cartItemService)
        {
            _cartItemService = cartItemService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCartItem() 
        {
            var values= await _cartItemService.GetAllCartItemAsync();
            return Ok(values);
        }

        [HttpPost("{CartId}")]
        public async Task<IActionResult> CreateCartItem(int CartId, [FromBody] CreateCartItemDto model) 
        {
            await _cartItemService.AddCartItemAsync(CartId,model);
            return Ok("Məhsullar Elavə olundu");
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetByIdCartItem(int Id) 
        {
            var value= await _cartItemService.GetByIdCartItemAsync(Id);
            return Ok(value);
        }

        [HttpDelete]

        public async Task<IActionResult> DeleteCartItem(int Id) 
        {
            await _cartItemService.DeleteCartItemAsync(Id);
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> UpdateCartItem([FromBody] UpdateCartItemDto model) 
        {
            if (model == null)
                return BadRequest("Data boşdur");
            await _cartItemService.UpdateCartItemAsync(model);
            return Ok(new { message = "Updated" });
        }
    }

}
