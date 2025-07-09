using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PerfumeStore.Application.Interfaces.IOrderRepository;
using PerfumeStore.Domain.Entities;
using PerfumeStore.Infrastructure.Data;
//using Stripe.Climate;

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
                .ThenInclude(oi=>oi.Product)
                .FirstOrDefaultAsync(o => o.OrderId == id);

        }

        public async Task<List<Order>> GetOrderWithOrderItems()
        {
            return await _context.Orders
                .Include(o => o.OrderItems) .ThenInclude(oi=>oi.Product) .ToListAsync();
        }
    }
}
