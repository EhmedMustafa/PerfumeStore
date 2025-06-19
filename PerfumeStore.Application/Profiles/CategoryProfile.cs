using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using PerfumeStore.Application.Dtos.CategoryDtos;
using PerfumeStore.Domain.Entities;

namespace PerfumeStore.Application.Profiles
{
   public class CategoryProfile :Profile
    {
        public CategoryProfile()
        {
            CreateMap<Category, ResultCategoryDto>().ReverseMap();
        }
    }
}
