using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PerfumeStore.Application.Dtos.OrderDto;
using PerfumeStore.Application.Dtos.OrderDtos;
using PerfumeStore.Application.Dtos.OrderItemDtos;
using PerfumeStore.Application.Interfaces;
using PerfumeStore.Application.Services.OrderServices;
using PerfumeStore.Domain.Entities;

namespace PerfumeStore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class OrderController : ControllerBase
    {
        //private readonly IGenericRepository<Order> _orderRepository;
        private readonly IOrderService _orderService;

        public OrderController(IGenericRepository<Order> orderRepository, IOrderService orderService)
        {
           // _orderRepository = orderRepository;
            _orderService = orderService;
        }

        [HttpGet]
       // [Authorize(Roles ="Admin")]
        public async Task<IActionResult> GetAll()
        {
            var orders = await _orderService.GetAllOrderAsync();
            return Ok(orders);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var order = await _orderService.GetOrderByIdAsync(id);
            if (order == null)
                return NotFound();

            return Ok(order);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateOrderDto createOrderDto)
        {
            await _orderService.CreateOrderAsync(createOrderDto);
            return Ok("Sifariş Uğurla əlavə olundu");
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateOrder([FromBody] List<ResultOrderItemDto> orderItems)
        {
            if (orderItems == null || orderItems.Count == 0)
                return BadRequest("Sifariş boş ola bilməz!");

            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var order = await _orderService.CreateOrderAsync(userId, orderItems);

            return Ok(order);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _orderService.DeleteOrderAsync(id);
            return Ok("Sifariş silindi");
        }


        [HttpPut]

        public async Task<IActionResult> Update(UpdateOrderDto updateOrderDto) 
        {
            await _orderService.UpdateOrderAsync(updateOrderDto);
            return Ok("Sifariş yenilendi");
        }

        //[HttpPut("{id}")]
        //public async Task<IActionResult> Update(int id, [FromBody] Order order)
        //{
        //    var existingOrder = await _orderRepository.GetByIdAsync(id);
        //    if (existingOrder == null)
        //        return NotFound();

        //    existingOrder.Status = order.Status;
        //    existingOrder.TotalAmount = order.TotalAmount;
        //    await _orderRepository.UpdateAsync(existingOrder);
        //    await _orderRepository.SaveChangesAsync();

        //    return NoContent();
        //}
    }
}
