﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PerfumeStore.Application.Dtos.BrandDtos;
using PerfumeStore.Domain.Entities;

namespace PerfumeStore.Application.Services.BrandServices
{
    public interface IBrandService
    {
        Task<IEnumerable<ResultBrandDto>> GetAllBrandAsync();
        Task<GetByIdBrandDto> GetByIdBrandAsync(int id);
        Task AddBrandAsync(CreateBrandDto createBrandDto);
        Task UpdateBrandAsync(UpdateBrandDto updateBrandDto);
        Task DeleteBrandAsync(int id);
        Task<ResultBrandDto> GetBrandDetailsWithProductsAsync(int brandId);
        Task<IEnumerable<IGrouping<string, BrandDto>>> GetGroupedBrandsAsync();
    }
}
