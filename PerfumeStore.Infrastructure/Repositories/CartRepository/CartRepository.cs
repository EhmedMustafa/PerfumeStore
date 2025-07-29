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

        public Task AddAsync(Cart cart)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Cart>> GetAllCartWithItemAsync()
        {
            return await _appDbContext.Carts
                .Include(c => c.CartItems).ThenInclude(ci => ci.Product).ToListAsync();
        }

        public async Task<Cart> GetCartByIdWithItemsAsync(int cartId)
        {
            var cart = await _appDbContext.Carts
                .Include(c => c.Customer)
                .Include(c => c.CartItems)
                    .ThenInclude(ci => ci.Product)
                .FirstOrDefaultAsync(c => c.CartId == cartId);

            return cart;
        }
     

        public async Task<Cart> GetCartByUserIdWithItemsAsync(int userId)
        {
            return await _appDbContext.Carts
                .Include(c => c.Customer)
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Product)
                .FirstOrDefaultAsync(c => c.CustomerId == userId);
        }
    }
}
