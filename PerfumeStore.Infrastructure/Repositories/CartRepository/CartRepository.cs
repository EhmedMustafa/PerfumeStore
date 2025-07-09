using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PerfumeStore.Application.Interfaces.ICartRepository;
using PerfumeStore.Domain.Entities;
using PerfumeStore.Infrastructure.Data;

namespace PerfumeStore.Infrastructure.Repositories.CartRepository
{
    public class CartRepository : ICartRepository
    {
        private readonly AppDbContext _appDbContext;

        public CartRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<Cart> GetCartWithItemsAsync(int cartId)
        {
            var cart = await _appDbContext.Carts
                .Include(c => c.CartItems)
                .FirstOrDefaultAsync(c => c.CartId == cartId);

            if (cart == null)
            {
                // Debug və ya logla xəbərdarlıq yaz
                Console.WriteLine($"Cart with ID {cartId} not found!");
            }

            return cart;
        }
    }
}
