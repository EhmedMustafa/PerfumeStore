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
            CreateMap<GetByIdProductDto, ResultProductDto>().ReverseMap();
            CreateMap<Product, GetByIdProductDto>().ReverseMap()
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(scr => scr.ImageUrl))
                .ForMember(dest => dest.Brand, opt => opt.MapFrom(src => src.Brand))
                .ForMember(dest => dest.Family, opt => opt.MapFrom(src => src.Family))
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category))
                .ForMember(dest => dest.ProductNotes, opt => opt.MapFrom(src => src.ProductNotes));




            CreateMap<Product, ResultProductDto>().ReverseMap()
                .ForMember(dest => dest.Brand, opt => opt.MapFrom(scr => scr.BrandName))
                .ForMember(dest => dest.Family, opt => opt.MapFrom(scr => scr.FamilyName))
                .ForMember(dest => dest.Category, opt => opt.MapFrom(scr => scr.CategoryName));

            CreateMap(typeof(PaginatedResult<>), typeof(PaginatedResult<>))
                                .ConvertUsing(typeof(PaginatedResultConverter<,>));

            CreateMap<ProductNote, ProductNoteDto>()
                .ForMember(dest => dest.FragranceNoteName, opt => opt.MapFrom(scr => scr.FragranceNote.Name));



        }
    }
}
