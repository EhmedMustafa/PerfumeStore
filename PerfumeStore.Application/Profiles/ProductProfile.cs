using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using PerfumeStore.Application.Dtos.ProductDtos;
using PerfumeStore.Domain.Entities;

namespace PerfumeStore.Application.Profiles
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<Product, UpdateProductDto>().ReverseMap();
            CreateMap<Product, ResultProductDto>().ReverseMap()
                .ForMember(dest => dest.Brand, opt => opt.MapFrom(scr => scr.BrandName))
                .ForMember(dest => dest.Family, opt => opt.MapFrom(scr => scr.FamilyName))
                .ForMember(dest => dest.Category, opt => opt.MapFrom(scr => scr.CategoryName));




            CreateMap<ProductNote, ProductNoteDto>()
                .ForMember(dest => dest.FragranceNoteName, opt => opt.MapFrom(scr => scr.FragranceNote.Name));

        }
    }
}
