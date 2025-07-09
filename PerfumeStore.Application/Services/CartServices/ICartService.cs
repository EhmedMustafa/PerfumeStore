using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PerfumeStore.Application.Dtos.CartDtos;

namespace PerfumeStore.Application.Services.CartServices
{
    public interface ICartService
    {
        Task<IEnumerable<ResultCartDto>> GetAllCartAsync();
        Task<GetByIdCartDto> GetByIdCartAsync(int id);
        Task AddCartAsync(CreateCartDto createCartDto);
       // Task UpdateCartAsync(UpdateCartDto updateCartDto);
        Task DeleteCartAsync(int id);
    }
}
