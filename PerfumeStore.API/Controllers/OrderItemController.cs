using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PerfumeStore.Application.Dtos.OrderItemDtos;
using PerfumeStore.Application.Interfaces;
using PerfumeStore.Application.Services.OrderItemServices;
using PerfumeStore.Application.Services.OrderServices;
using PerfumeStore.Domain.Entities;

namespace PerfumeStore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderItemController : ControllerBase
    {
        private readonly IOrderItemService _orderItemService;
        private readonly IOrderService _order;

        public OrderItemController(IOrderItemService orderItemService,IOrderService order)
        {
            _orderItemService = orderItemService;
            _order = order;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var orderItems = await _orderItemService.GetAllOrderItemAsync();
            return Ok(orderItems);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var orderItem = await _orderItemService.GetByIdOrderItemAsync(id);
            if (orderItem == null)
                return NotFound();

            return Ok(orderItem);
        }

        [HttpPost("{orderId}")]
        public async Task<IActionResult> Create(int orderId, [FromBody] CreateOrderItemDto createOrderItemDto)
        {
            
            await _orderItemService.CreateOrderItemAsync(orderId, createOrderItemDto);
            return Ok("OrderItem yüklendi");
        }

        [HttpPut]
        public async Task<IActionResult> Update(UpdateOrderItemDto orderItem)
        {
            await _orderItemService.UpdateOrderItemAsync(orderItem);
            return Ok("OrderItem yeniləndi");
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            await _orderItemService.DeleteOrderItemAsync(id);
            return Ok("OrderItem silindi");
        }
    }
}
