using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using PerfumeStore.Application.Interfaces;
using PerfumeStore.Application.Services;
using PerfumeStore.Domain.Entities;

namespace PerfumeStore.Infrastructure.Services
{
    public class OrderItemService : IOrderItemService
    {
        private readonly IGenericRepository<OrderItem> _orderItemRepository;

        public OrderItemService(IGenericRepository<OrderItem> orderItemRepository)
        {
            _orderItemRepository = orderItemRepository;
        }

        public async Task<IEnumerable<OrderItem>> GetAllAsync()
        {
            return await _orderItemRepository.GetAllAsync();
        }

        public async Task<OrderItem> GetByIdAsync(int id)
        {
            return await _orderItemRepository.GetByIdAsync(id);
        }

        public async Task AddAsync(OrderItem orderItem)
        {
            await _orderItemRepository.AddAsync(orderItem);
            await _orderItemRepository.SaveChangesAsync();
        }

        public async Task UpdateAsync(OrderItem orderItem)
        {
            await _orderItemRepository.UpdateAsync(orderItem);
            await _orderItemRepository.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var orderItem = await _orderItemRepository.GetByIdAsync(id);
            if (orderItem != null)
            {
                await _orderItemRepository.DeleteAsync(orderItem);
                await _orderItemRepository.SaveChangesAsync();
            }
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
