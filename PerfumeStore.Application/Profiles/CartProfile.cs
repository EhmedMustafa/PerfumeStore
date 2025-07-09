using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using PerfumeStore.Application.Dtos.CartDtos;
using PerfumeStore.Domain.Entities;

namespace PerfumeStore.Application.Profiles
{
   public class CartProfile:Profile
    {
        public CartProfile()
        {
                CreateMap<Cart,ResultCartDto>().ReverseMap();
                CreateMap<Cart,GetByIdCartDto>().ReverseMap();
        }
    }
}
