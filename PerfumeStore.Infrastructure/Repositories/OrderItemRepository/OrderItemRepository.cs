using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PerfumeStore.Application.Interfaces.IOrderItemRepository;
using PerfumeStore.Domain.Entities;
using PerfumeStore.Infrastructure.Data;

namespace PerfumeStore.Infrastructure.Repositories.OrderItemRepository
{
    public class OrderItemRepository : IOrderItemRepository
    {
        private readonly AppDbContext _context;

        public OrderItemRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<OrderItem>> GetOrderItemWithDetails()
        {
            return await _context.OrderItems
                .Include(p => p.Product).ToListAsync();
        }
    }
}
