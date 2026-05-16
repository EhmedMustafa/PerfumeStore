using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PerfumeStore.Application.Dtos.OrderDto;
using PerfumeStore.Application.Dtos.OrderDtos;
using PerfumeStore.Application.Services.OrderServices;

namespace PerfumeStore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        // GET /api/Order — Bütün sifarişlər (yalnız Admin)
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAll()
        {
            var orders = await _orderService.GetAllOrderAsync();
            return Ok(orders);
        }

        // GET /api/Order/{id} — Yalnız öz sifarişini və ya Admin
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var order = await _orderService.GetOrderByIdAsync(id);
            if (order == null) return NotFound();

            var currentUserId = GetCurrentUserId();
            var isAdmin = User.IsInRole("Admin");
            if (!isAdmin && order.UserId != currentUserId)
                return Forbid();

            return Ok(order);
        }

        // POST /api/Order — Sifariş yarat (login məcburidir)
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateOrderDto dto)
        {
            try
            {
                var userId = GetCurrentUserId();
                var orderId = await _orderService.CreateOrderAsync(userId, dto);
                return Ok(new { orderId, message = "Sifariş uğurla qeydə alındı" });
            }
            catch (System.InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // GET /api/Order/my — İstifadəçinin öz sifarişləri
        [HttpGet("my")]
        public async Task<IActionResult> GetMyOrders()
        {
            var userId = GetCurrentUserId();
            var orders = await _orderService.GetOrdersByUserIdAsync(userId);
            return Ok(orders);
        }

        // PUT /api/Order — Status yenilə (yalnız Admin)
        [HttpPut]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(UpdateOrderDto dto)
        {
            await _orderService.UpdateOrderAsync(dto);
            return Ok(new { message = "Sifariş yeniləndi" });
        }

        // DELETE /api/Order/{id} — Yalnız Admin
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            await _orderService.DeleteOrderAsync(id);
            return Ok(new { message = "Sifariş silindi" });
        }

        private int GetCurrentUserId()
        {
            var claim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return int.Parse(claim);
        }
    }
}
