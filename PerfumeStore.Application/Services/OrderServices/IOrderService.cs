using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PerfumeStore.Application.Dtos.OrderDto;
using PerfumeStore.Application.Dtos.OrderDtos;
using PerfumeStore.Application.Dtos.OrderItemDtos;
using PerfumeStore.Domain.Entities;
using PerfumeStore.Domain.Enums;

namespace PerfumeStore.Application.Services.OrderServices
{
    public interface IOrderService
    {
        Task<IEnumerable<ResultOrderDto>> GetAllOrderAsync();
        Task<GetByIdOrderDto> GetOrderByIdAsync(int Id);// Verilən ID-yə əsasən sifarişi tapır
        Task CreateOrderAsync(CreateOrderDto createOrderDto);
        Task UpdateOrderAsync(UpdateOrderDto updateOrderDto);
        Task DeleteOrderAsync(int Id);


        Task<Order> CreateOrderAsync(int userId, List<ResultOrderItemDto> orderItems);//Yeni sifariş yaradır. İstifadəçi ID-si və sifariş məhsulları (OrderItemDto) gəlir.
        
        Task<IEnumerable<Order>> GetOrdersByUserIdAsync(int userId);//İstifadəçinin bütün sifarişlərini qaytarır

        Task<bool> UpdateOrderStatusAsync(int orderId, OrderStatus status);//Sifarişin statusunu dəyişir (Pending, Completed və s.).
    }
}
