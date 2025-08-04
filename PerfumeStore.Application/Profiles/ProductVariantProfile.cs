using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using PerfumeStore.Application.Dtos.ProductVariantDtos;
using PerfumeStore.Domain.Entities;

namespace PerfumeStore.Application.Profiles
{
    public class ProductVariantProfile : Profile
    {
        public ProductVariantProfile()
        {
            CreateMap<ProductVariant, ResultProductVariantDto>();
            CreateMap<Product, ResultProductVariantDto>().ReverseMap();
            CreateMap<ProductVariant, ProductVariantCreateDto>();


        }
    }
}
