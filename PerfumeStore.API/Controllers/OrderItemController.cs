using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PerfumeStore.Application.Interfaces;
using PerfumeStore.Domain.Entities;

namespace PerfumeStore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderItemController : ControllerBase
    {
        private readonly IGenericRepository<OrderItem> _orderItemRepository;

        public OrderItemController(IGenericRepository<OrderItem> orderItemRepository)
        {
            _orderItemRepository = orderItemRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var orderItems = await _orderItemRepository.GetAllAsync();
            return Ok(orderItems);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var orderItem = await _orderItemRepository.GetByIdAsync(id);
            if (orderItem == null)
                return NotFound();

            return Ok(orderItem);
        }

        [HttpPost]
        //public async Task<IActionResult> Create([FromBody] OrderItem orderItem)
        //{
        //    await _orderItemRepository.AddAsync(orderItem);
        //    await _orderItemRepository.SaveChangesAsync();
        //    return CreatedAtAction(nameof(GetById), new { id = orderItem.Id }, orderItem);
        //}

        [HttpPut("{id}")]
        //public async Task<IActionResult> Update(int id, [FromBody] OrderItem orderItem)
        //{
        //    var existingOrderItem = await _orderItemRepository.GetByIdAsync(id);
        //    if (existingOrderItem == null)
        //        return NotFound();

        //    existingOrderItem.Quantity = orderItem.Quantity;
        //    existingOrderItem.Price = orderItem.Price;
        //    await _orderItemRepository.UpdateAsync(existingOrderItem);
        //    await _orderItemRepository.SaveChangesAsync();

        //    return NoContent();
        //}

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var orderItem = await _orderItemRepository.GetByIdAsync(id);
            if (orderItem == null)
                return NotFound();

            await _orderItemRepository.DeleteAsync(orderItem);
            await _orderItemRepository.SaveChangesAsync();

            return NoContent();
        }
    }
}
