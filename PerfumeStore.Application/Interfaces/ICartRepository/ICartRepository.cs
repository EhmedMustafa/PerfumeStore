using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PerfumeStore.Domain.Entities;

namespace PerfumeStore.Application.Interfaces.ICartRepository
{
    public interface ICartRepository
    {
        Task<Cart> GetCartByIdWithItemsAsync(int cartId);
        Task<List<Cart>> GetAllCartWithItemAsync();

        Task<Cart?> GetCartByUserIdWithItemsAsync(int userId);
        Task AddAsync(Cart cart);

    }
}
