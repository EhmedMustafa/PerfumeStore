using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PerfumeStore.Application.Interfaces.IOrderRepository;
using PerfumeStore.Domain.Entities;
using PerfumeStore.Infrastructure.Data;

namespace PerfumeStore.Infrastructure.Repositories.OrderRepository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly AppDbContext _context;

        public OrderRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Order> GetOrderByIdWithOrderItems(int id)
        {
            return await _context.Orders
                 .Include(o => o.OrderItems)
                 .ThenInclude(oi => oi.Product)
                 .FirstOrDefaultAsync(o => o.OrderId == id);
        }

        public async Task<List<Order>> GetOrderWithOrderItems()
        {
            return await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .ToListAsync();
        }

        public async Task<List<Product>> GetProductsWithVariantsAsync(List<int> productIds)
        {
            if (productIds == null || productIds.Count == 0)
                return new List<Product>();

            return await _context.Products
                .Where(p => productIds.Contains(p.ProductId))
                .Include(p => p.Brand)
                .Include(p => p.ProductVariants)
                .ToListAsync();
        }
    }
}
