using System.Collections.Generic;
using System.Threading.Tasks;
using PerfumeStore.Application.Dtos.OrderDto;
using PerfumeStore.Application.Dtos.OrderDtos;
using PerfumeStore.Domain.Entities;
using PerfumeStore.Domain.Enums;

namespace PerfumeStore.Application.Services.OrderServices
{
    public interface IOrderService
    {
        Task<IEnumerable<ResultOrderDto>> GetAllOrderAsync();
        Task<GetByIdOrderDto> GetOrderByIdAsync(int id);

        // Sifariş yaradılır — UserId token-dən gəlir (controller-dən ötürülür)
        Task<int> CreateOrderAsync(int userId, CreateOrderDto dto);

        Task UpdateOrderAsync(UpdateOrderDto updateOrderDto);
        Task DeleteOrderAsync(int id);

        // Müştərinin öz sifarişləri
        Task<IEnumerable<ResultOrderDto>> GetOrdersByUserIdAsync(int userId);

        Task<bool> UpdateOrderStatusAsync(int orderId, OrderStatus status);
    }
}
