using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using PerfumeStore.Application.Dtos.OrderDto;
using PerfumeStore.Application.Dtos.OrderDtos;
using PerfumeStore.Application.Dtos.OrderItemDtos;
using PerfumeStore.Application.Interfaces;
using PerfumeStore.Application.Services.OrderServices;
using PerfumeStore.Domain.Entities;
using PerfumeStore.Domain.Enums;

namespace PerfumeStore.Infrastructure.Services
{
    public class OrderService : IOrderService
    {
        private readonly IGenericRepository<Order> _orderRepository;
        private readonly IGenericRepository<OrderItem> _orderItemRepository;
        private readonly IGenericRepository<Product> _productRepository;
        private readonly IMapper _mapper;

        public OrderService(IGenericRepository<Order> orderRepository, IGenericRepository<OrderItem> orderItemRepository,
        IGenericRepository<Product> productRepository,IMapper mapper)
        {
            _orderRepository = orderRepository;
            _orderItemRepository = orderItemRepository;
            _productRepository = productRepository;
            _mapper = mapper;
        }


        public async Task<IEnumerable<Order>> FindAsync(Expression<Func<Order, bool>> predicate)
        {
            return await _orderRepository.FindAsync(predicate);
        }
        public async Task<Order> CreateOrderAsync(int userId, List<ResultOrderItemDto> orderItems)
        {
            // 1️⃣ Yeni sifariş obyekti yaradılır.
            var order = new Order
            {
                UserId = userId,              // İstifadəçi ID-si sifarişə bağlanır.
                OrderDate = DateTime.UtcNow,  // Sifarişin yaradılma tarixi.
                Status = OrderStatus.Pending, // Başlanğıc status (Gözləyən/Pending).
                TotalAmount = 0               // Ümumi məbləğ sıfırdan başlayır.
            };

            var orderItemsList = new List<OrderItem>(); // Sifariş məhsullarını saxlamaq üçün siyahı.

            // 2️⃣ Gələn hər bir məhsulu yoxlayırıq və sifarişə əlavə edirik.
            foreach (var item in orderItems)
            {
                // 2.1. Məhsulun bazada olub-olmadığını yoxlayırıq.
                var product = await _productRepository.GetByIdAsync(item.ProductId);
                if (product == null)
                    throw new Exception("Product not found!"); // Məhsul yoxdursa, xəta qaytarılır.

                // 2.2. Yeni OrderItem (Sifariş Məhsulu) obyekti yaradılır.
                var orderItem = new OrderItem
                {
                    ProductId = product.ProductId,   // Məhsulun ID-si
                    Quantity = item.Quantity, // Sifariş olunan miqdar
                    TotalPrice = product.OriginalPrice,    // Məhsulun cari qiyməti
                    Order = order             // Sifarişə bağlanır
                };

                // 2.3. Sifarişin ümumi məbləği yenilənir.
                order.TotalAmount += orderItem.Quantity * orderItem.TotalPrice;

                // 2.4. Sifariş məhsulu siyahıya əlavə edilir.
                orderItemsList.Add(orderItem);
            }

            // 3️⃣ Sifariş məhsulları order obyektinə əlavə edilir.
            order.OrderItems = orderItemsList;

            // 4️⃣ Sifarişi bazaya əlavə edirik və yadda saxlayırıq.
            await _orderRepository.AddAsync(order);
            await _orderRepository.SaveChangesAsync();

            // 5️⃣ Yaradılmış sifariş qaytarılır.
            return order;
        }
      
        public async Task<IEnumerable<Order>> GetOrdersByUserIdAsync(int userId)
        {
            return await _orderRepository.FindAsync(o => o.UserId == userId);
        }
        public async Task<bool> UpdateOrderStatusAsync(int orderId, OrderStatus status)
        {
            var order = await _orderRepository.GetByIdAsync(orderId);
            if (order == null)
                return false;

            order.Status = status;
            await _orderRepository.UpdateAsync(order);
            await _orderRepository.SaveChangesAsync();

            return true;
        }

        public async Task CreateOrderAsync(CreateOrderDto createOrderDto)
        {
            await _orderRepository.AddAsync(new Order
            {
                OrderDate = createOrderDto.OrderDate,
                TotalAmount=createOrderDto.TotalAmount,
                Status=createOrderDto.Status,
                UserId=createOrderDto.UserId
            });
            await _orderRepository.SaveChangesAsync();
        }

        public async Task<IEnumerable<ResultOrderDto>> GetAllOrderAsync()
        {
            var vallues= await _orderRepository.GetAllAsync();
            var map = _mapper.Map<IEnumerable<ResultOrderDto>>(vallues);
            return map;
        }

        async Task<GetByIdOrderDto> IOrderService.GetOrderByIdAsync(int Id)
        {
            var values = await _orderRepository.GetByIdAsync(Id);
            var map= _mapper.Map<GetByIdOrderDto>(values);
            return map;
        }

      

        public async Task UpdateOrderAsync(UpdateOrderDto updateOrderDto)
        {
            var values = await _orderRepository.GetByIdAsync(updateOrderDto.OrderId);

            values.OrderDate = updateOrderDto.OrderDate;
            values.TotalAmount = updateOrderDto.TotalAmount;
            values.Status=updateOrderDto.Status;
            values.UserId = updateOrderDto.UserId;

            await _orderRepository.UpdateAsync(values);
            await _orderRepository.SaveChangesAsync();
        }

        public async Task DeleteOrderAsync(int Id)
        {
            var values= await _orderRepository.GetByIdAsync(Id);
            await _orderRepository.DeleteAsync(values);
            await _orderRepository.SaveChangesAsync();
        }
    }
}
