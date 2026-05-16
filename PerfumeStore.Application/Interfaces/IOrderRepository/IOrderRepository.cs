using System.Collections.Generic;
using System.Threading.Tasks;
using PerfumeStore.Domain.Entities;

namespace PerfumeStore.Application.Interfaces.IOrderRepository
{
    public interface IOrderRepository
    {
        Task<Order> GetOrderByIdWithOrderItems(int id);
        Task<List<Order>> GetOrderWithOrderItems();

        // Sifariş yaratmaq üçün məhsulları variant və brand ilə yüklə
        Task<List<Product>> GetProductsWithVariantsAsync(List<int> productIds);
    }
}
