using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using PerfumeStore.Application.Dtos.OrderItemDtos;
using PerfumeStore.Application.Interfaces;
using PerfumeStore.Application.Interfaces.IOrderItemRepository;
using PerfumeStore.Application.Services.OrderItemServices;
using PerfumeStore.Domain.Entities;

namespace PerfumeStore.Infrastructure.Services
{
    public class OrderItemService : IOrderItemService
    {
        private readonly IGenericRepository<OrderItem> _orderItemRepository;
        private readonly IGenericRepository<Order> _order;
        private readonly IGenericRepository<Product> _product;
        private readonly IOrderItemRepository _orderitem;
        private readonly IMapper _mapper;

        public OrderItemService(IGenericRepository<OrderItem> orderItemRepository, IMapper mapper, IGenericRepository<Order> order, IGenericRepository<Product> Product, IOrderItemRepository orderitem)
        {
            _orderItemRepository = orderItemRepository;
            _mapper = mapper;
            _order = order;
            _product = Product;
            _orderitem = orderitem;
        }

        public async Task CreateOrderItemAsync(int orderId, CreateOrderItemDto orderItem)
        {
            var order = await _order.GetByIdAsync(orderId);
            if (order == null) throw new Exception("Sifariş tapılmadı");

            var product = await _product.GetByIdAsync(orderItem.ProductId);
            if (product == null)
                throw new Exception("Məhsul tapılmadı");

            var ordetitems = new OrderItem
            {
                OrderId = orderId,
                ProductId = orderItem.ProductId,
                Quantity = orderItem.Quantity,
               // TotalPrice = product.OriginalPrice * orderItem.Quantity
            };

            await _orderItemRepository.AddAsync(ordetitems);

            order.TotalAmount += ordetitems.TotalPrice;
          
            await _orderItemRepository.SaveChangesAsync();
            await _order.SaveChangesAsync();
        }

        public async Task<IEnumerable<ResultOrderItemDto>> GetAllOrderItemAsync()
        {
            var values= await _orderitem.GetOrderItemWithDetails();
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
