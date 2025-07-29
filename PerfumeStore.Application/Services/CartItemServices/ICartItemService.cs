using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PerfumeStore.Application.Dtos.CartItemDtos;

namespace PerfumeStore.Application.Services.CartItemItemServices
{
    public interface ICartItemService
    {
        Task<IEnumerable<ResultCartItemDto>> GetAllCartItemAsync();
        Task<GetByIdCartItemDto> GetByIdCartItemAsync(int id);
        Task AddCartItemAsync(int cartId, CreateCartItemDto createCartItemDto);
        Task <bool> UpdateCartItemAsync(UpdateCartItemDto updateCartItemDto);
        Task<bool> DeleteCartItemAsync(int id);
    }
}
