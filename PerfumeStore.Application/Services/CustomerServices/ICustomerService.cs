using PerfumeStore.Application.Dtos.CategoryDtos;
using PerfumeStore.Application.Dtos.CustomerDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PerfumeStore.Application.Services.CustomerServices
{
    public interface ICustomerService
    {
        Task<IEnumerable<ResultCustomerDto>> GetAllCustomerAsync();
        Task<GetByIdCustomerDto> GetByIdCustomerAsync(int id);
        Task AddCustomerAsync(CreateCustomerDto createCustomerDto);
        Task UpdateCustomerAsync(UpdateCustomerDto updateCustomerDto);
        Task DeleteCustomerAsync(int id); 
    }
}
