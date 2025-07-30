using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using PerfumeStore.Application.Dtos.CartItemDtos;
using PerfumeStore.Domain.Entities;

namespace PerfumeStore.Application.Profiles
{
    public class CartItemProfile:Profile
    {
        public CartItemProfile()
        {
            CreateMap<CartItem, ResultCartItemDto>();
                //.ForMember(dest => dest.UnitPrice, opt => opt.MapFrom(scr => scr.Product.OriginalPrice))

                //.ForMember(dest => dest.ProductName, opt => opt.MapFrom(scr => scr.Product));
            CreateMap<CartItem, GetByIdCartItemDto>().ReverseMap();
                
        }
    }
}
