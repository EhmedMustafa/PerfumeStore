using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using PerfumeStore.Application.Dtos.OrderItemDtos;
using PerfumeStore.Application.Interfaces;
using PerfumeStore.Application.Services.OrderItemServices;
using PerfumeStore.Domain.Entities;

namespace PerfumeStore.Infrastructure.Services
{
    public class OrderItemService : IOrderItemService
    {
        private readonly IGenericRepository<OrderItem> _orderItemRepository;
        private readonly IMapper _mapper;

        public OrderItemService(IGenericRepository<OrderItem> orderItemRepository,IMapper mapper)
        {
            _orderItemRepository = orderItemRepository;
            _mapper = mapper;
        }

        public async Task CreateOrderItemAsync(CreateOrderItemDto orderItem)
        {
            await _orderItemRepository.AddAsync(new OrderItem
            {
               //OrderId = orderItem.OrderId,
               ProductId = orderItem.ProductId,
               Quantity = orderItem.Quantity,
               TotalPrice = orderItem.TotalPrice,
            });
            await _orderItemRepository.SaveChangesAsync();
        }

        public async Task<IEnumerable<ResultOrderItemDto>> GetAllOrderItemAsync()
        {
            var values= await _orderItemRepository.GetAllAsync();
            var map = _mapper.Map<IEnumerable<ResultOrderItemDto>>(values);
            return map;
        }
        public async Task<GetByIdOrderItemDto> GetByIdOrderItemAsync(int id)
        {
            var values= await _orderItemRepository.GetByIdAsync(id);
            var map = _mapper.Map<GetByIdOrderItemDto>(values);
            return map;
        }

        public async Task UpdateOrderItemAsync(UpdateOrderItemDto orderItem)
        {
            var values = await _orderItemRepository.GetByIdAsync(orderItem.OrderId);
            values.OrderId = orderItem.OrderId;
            values.ProductId = orderItem.ProductId;
            values.Quantity = orderItem.Quantity;
            values.TotalPrice = orderItem.TotalPrice;

            await _orderItemRepository.UpdateAsync(values);
            await _orderItemRepository.SaveChangesAsync();
        }
        public async Task DeleteOrderItemAsync(int id)
        {
            var values = await _orderItemRepository.GetByIdAsync(id);
            await _orderItemRepository.DeleteAsync(values);
            await _orderItemRepository.SaveChangesAsync();
        }

     
        public async Task<IEnumerable<OrderItem>> FindAsync(Expression<Func<OrderItem, bool>> predicate)
        {
            return await _orderItemRepository.FindAsync(predicate);
        }

       

        public async Task<IEnumerable<OrderItem>> GetOrderItemsByOrderIdAsync(int orderId)
        {
            return await _orderItemRepository.FindAsync(oi => oi.OrderId == orderId);
        }

      
    }
}
