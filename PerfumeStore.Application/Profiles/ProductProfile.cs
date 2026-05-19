using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
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
            CreateMap<Product, GetByIdProductDto>()
                .ForMember(d => d.GalleryImages, opt => opt.MapFrom(s => DeserializeGallery(s.GalleryImagesJson)))
                .ReverseMap()
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(scr => scr.ImageUrl))
                .ForMember(dest => dest.Brand, opt => opt.MapFrom(src => src.Brand))
                .ForMember(dest => dest.Family, opt => opt.MapFrom(src => src.Family))
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category))
                .ForMember(dest => dest.ProductNotes, opt => opt.MapFrom(src => src.ProductNotes));




            CreateMap<Product, ResultProductDto>()
                .ForMember(d => d.GalleryImages, opt => opt.MapFrom(s => DeserializeGallery(s.GalleryImagesJson)))
                .ReverseMap()
                .ForMember(dest => dest.Brand, opt => opt.MapFrom(scr => scr.BrandName))
                .ForMember(dest => dest.Family, opt => opt.MapFrom(scr => scr.FamilyName))
                .ForMember(dest => dest.Category, opt => opt.MapFrom(scr => scr.CategoryName));

            CreateMap(typeof(PaginatedResult<>), typeof(PaginatedResult<>))
                                .ConvertUsing(typeof(PaginatedResultConverter<,>));

            CreateMap<ProductNote, ProductNoteDto>()
                .ForMember(dest => dest.FragranceNoteName, opt => opt.MapFrom(scr => scr.FragranceNote.Name));



        }

        // GalleryImagesJson (string) → List<string>
        private static List<string> DeserializeGallery(string json)
        {
            if (string.IsNullOrWhiteSpace(json)) return new List<string>();
            try { return JsonSerializer.Deserialize<List<string>>(json) ?? new List<string>(); }
            catch { return new List<string>(); }
        }
    }
}
