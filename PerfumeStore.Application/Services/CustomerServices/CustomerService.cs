using AutoMapper;
using PerfumeStore.Application.Dtos.CustomerDtos;
using PerfumeStore.Application.Interfaces;
using PerfumeStore.Domain.Entities;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Customer = PerfumeStore.Domain.Entities.Customer;

namespace PerfumeStore.Application.Services.CustomerServices
{
    public class CustomerService : ICustomerService
    {
        private readonly IGenericRepository<Customer> _genericRepository;
        private readonly IMapper _mapper;

        public CustomerService(IGenericRepository<Customer> genericRepository, IMapper mapper)
        {
            _genericRepository = genericRepository;
            _mapper = mapper;
        }

        public async Task AddCustomerAsync(CreateCustomerDto createCustomerDto)
        {
            await _genericRepository.AddAsync(new Customer
            {
                FirstName=createCustomerDto.FirstName,
                LastName=createCustomerDto.LastName,
                Email=createCustomerDto.Email,
            });
            await _genericRepository.SaveChangesAsync();
        }

        public async Task DeleteCustomerAsync(int id)
        {
            var values = await _genericRepository.GetByIdAsync(id);
            if (values != null) 
            {
                await _genericRepository.DeleteAsync(values);
                await _genericRepository.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<ResultCustomerDto>> GetAllCustomerAsync()
        {
          var getall= await _genericRepository.GetAllAsync();
          var map= _mapper.Map<IEnumerable<ResultCustomerDto>>(getall);
          return map;

        }

        public async Task<GetByIdCustomerDto> GetByIdCustomerAsync(int id)
        {
            var values= await _genericRepository.GetByIdAsync(id);
            var map = _mapper.Map<GetByIdCustomerDto>(values);
            return map;
        }

        public async Task UpdateCustomerAsync(UpdateCustomerDto updateCustomerDto)
        {
            var values = await _genericRepository.GetByIdAsync(updateCustomerDto.CustomerId);
            values.FirstName = updateCustomerDto.FirstName;
            values.LastName = updateCustomerDto.LastName;
            values.Email = updateCustomerDto.Email;
            await _genericRepository.UpdateAsync(values);
            await _genericRepository.SaveChangesAsync(); 
        }
    }
}
