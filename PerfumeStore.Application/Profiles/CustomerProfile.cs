using AutoMapper;
using PerfumeStore.Application.Dtos.CustomerDtos;
using PerfumeStore.Domain.Entities;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Customer = PerfumeStore.Domain.Entities.Customer;

namespace PerfumeStore.Application.Profiles
{
    public class CustomerProfile:Profile
    {
        public CustomerProfile()
        {
                CreateMap<Customer, ResultCustomerDto>().ReverseMap();
                CreateMap<Customer, GetByIdCustomerDto>().ReverseMap();
        }
    }
}
