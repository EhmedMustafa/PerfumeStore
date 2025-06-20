using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PerfumeStore.Application.Dtos.OrderItemDtos;
using PerfumeStore.Application.Interfaces;
using PerfumeStore.Application.Services;
using PerfumeStore.Domain.Entities;

namespace PerfumeStore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrderController : ControllerBase
    {
        private readonly IGenericRepository<Order> _orderRepository;
        private readonly IOrderService _orderService;

        public OrderController(IGenericRepository<Order> orderRepository, IOrderService orderService)
        {
            _orderRepository = orderRepository;
            _orderService = orderService;
        }

        [HttpGet("All")]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> GetAll()
        {
            var orders = await _orderRepository.GetAllAsync();
            return Ok(orders);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var order = await _orderRepository.GetByIdAsync(id);
            if (order == null)
                return NotFound();

            return Ok(order);
        }

        [HttpPost]
        //public async Task<IActionResult> Create([FromBody] Order order)
        //{
        //    await _orderRepository.AddAsync(order);
        //    await _orderRepository.SaveChangesAsync();
        //    return CreatedAtAction(nameof(GetById), new { id = order.Id }, order);
        //}
        [HttpPost("create")]
        public async Task<IActionResult> CreateOrder([FromBody] List<OrderItemDto> orderItems)
        {
            if (orderItems == null || orderItems.Count == 0)
                return BadRequest("Sifariş boş ola bilməz!");

            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var order = await _orderService.CreateOrderAsync(userId, orderItems);

            return Ok(order);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Order order)
        {
            var existingOrder = await _orderRepository.GetByIdAsync(id);
            if (existingOrder == null)
                return NotFound();

            existingOrder.Status = order.Status;
            existingOrder.TotalAmount = order.TotalAmount;
            await _orderRepository.UpdateAsync(existingOrder);
            await _orderRepository.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var order = await _orderRepository.GetByIdAsync(id);
            if (order == null)
                return NotFound();

            await _orderRepository.DeleteAsync(order);
            await _orderRepository.SaveChangesAsync();

            return NoContent();
        }
    }
}
