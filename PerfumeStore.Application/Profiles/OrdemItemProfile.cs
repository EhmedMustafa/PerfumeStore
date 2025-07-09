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
            CreateMap<OrderItem, ResultOrderItemDto>()
            .ForMember(dest => dest.ProductName, opt => opt.MapFrom(scr => scr.Product.Name));
                
                CreateMap<OrderItem,GetByIdOrderItemDto>().ReverseMap();
        }
    }
}
