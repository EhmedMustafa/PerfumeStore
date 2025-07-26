using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using PerfumeStore.Application.Dtos.BrandDtos;
using PerfumeStore.Domain.Entities;

namespace PerfumeStore.Application.Profiles 
{ 
      public   class BrandProfile:Profile
    {
        public BrandProfile()
        {
            CreateMap<Brand, ResultBrandDto>()
            .ForMember(dest => dest.products, opt => opt.MapFrom(scr => scr.products));
                CreateMap<Brand,GetByIdBrandDto>().ReverseMap();
            CreateMap<Brand, BrandDto>().ReverseMap();
        }

    }
}
