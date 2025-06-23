using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PerfumeStore.Application.Dtos.OrderItemDtos;
using PerfumeStore.Domain.Entities;

namespace PerfumeStore.Application.Services.OrderItemServices
{
    public interface IOrderItemService
    {
        Task<IEnumerable<ResultOrderItemDto>> GetAllOrderItemAsync();
        Task<GetByIdOrderItemDto> GetByIdOrderItemAsync(int id);
        Task CreateOrderItemAsync(CreateOrderItemDto orderItem);
        Task UpdateOrderItemAsync(UpdateOrderItemDto orderItem);
        Task DeleteOrderItemAsync(int id);
        Task<IEnumerable<OrderItem>> GetOrderItemsByOrderIdAsync(int orderId);// Bir sifarişə aid olan bütün məhsulları qaytarır
    }
}
