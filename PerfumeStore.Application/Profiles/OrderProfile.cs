using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using PerfumeStore.Application.Dtos.OrderDto;
using PerfumeStore.Application.Dtos.OrderDtos;
using PerfumeStore.Domain.Entities;
using Stripe.Climate;
using Order = PerfumeStore.Domain.Entities.Order;

namespace PerfumeStore.Application.Profiles
{
    public class OrderProfile:Profile
    {
        public OrderProfile()
        {
               CreateMap<Order,ResultOrderDto>().ReverseMap();
               CreateMap<Order,GetByIdOrderDto>().ReverseMap();
        }
    }
}
