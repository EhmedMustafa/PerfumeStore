using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PerfumeStore.Application.Dtos.OrderItemDtos;
using PerfumeStore.Domain.Entities;
using PerfumeStore.Domain.Enums;

namespace PerfumeStore.Application.Services
{
    public interface IOrderService
    {
        Task<IEnumerable<Order>> GetAllAsync();
       
        Task AddAsync(Order order);
        Task UpdateAsync(Order order);
        Task DeleteAsync(int id);
        Task<Order> CreateOrderAsync(int userId, List<OrderItemDto> orderItems);//Yeni sifariş yaradır. İstifadəçi ID-si və sifariş məhsulları (OrderItemDto) gəlir.
        Task<Order> GetOrderByIdAsync(int orderId);// Verilən ID-yə əsasən sifarişi tapır
        Task<IEnumerable<Order>> GetOrdersByUserIdAsync(int userId);//İstifadəçinin bütün sifarişlərini qaytarır
        Task<bool> UpdateOrderStatusAsync(int orderId, OrderStatus status);//Sifarişin statusunu dəyişir (Pending, Completed və s.).
    }
}
