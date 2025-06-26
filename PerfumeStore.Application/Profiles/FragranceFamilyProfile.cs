using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using PerfumeStore.Application.Dtos.FragrancefamilyDtos;
using PerfumeStore.Domain.Entities;

namespace PerfumeStore.Application.Profiles
{
    public class FragranceFamilyProfile:Profile
    {
        public FragranceFamilyProfile()
        {
                CreateMap<FragranceFamily,ResultFragranceFamilyDto>().ReverseMap();
                CreateMap<FragranceFamily, GetByIdFragranceFamilyDto>().ReverseMap();
        }
    }
}
