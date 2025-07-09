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
        Task<Cart> GetCartWithItemsAsync(int cartId);
    }
}
