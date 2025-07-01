using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using PerfumeStore.Application.Dtos.FragranceNoteDtos;
using PerfumeStore.Domain.Entities;
using Stripe;

namespace PerfumeStore.Application.Profiles
{
    public class FragranceNoteProfile:Profile
    {
        public FragranceNoteProfile()
        {
            CreateMap<FragranceNote,ResultFragranceNoteDto>().ReverseMap();
          
            CreateMap<FragranceNote, GetByIdFragranceNoteDto>()
            .ForMember(dest => dest.Types, opt => opt.MapFrom(scr => scr.NoteTypes.Select(nt => nt.Type).ToList()))
            .ForMember(dest => dest.Products, opt => opt.MapFrom(scr => scr.ProductNotes.Select(nt => nt.Product)));

        }
    }
}
