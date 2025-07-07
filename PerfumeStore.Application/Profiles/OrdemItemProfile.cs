using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using PerfumeStore.Application.Dtos.OrderItemDtos;
using PerfumeStore.Domain.Entities;

namespace PerfumeStore.Application.Profiles
{
    internal class OrdemItemProfile:Profile
    {
        public OrdemItemProfile()
        {
            CreateMap<OrderItem, ResultOrderItemDto>().ReverseMap()
            .ForMember(dest => dest.Product, opt => opt.MapFrom(scr => scr.ProductName));
                
                CreateMap<OrderItem,GetByIdOrderItemDto>().ReverseMap();
        }
    }
}
