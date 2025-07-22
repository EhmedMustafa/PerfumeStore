using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PerfumeStore.Application.Dtos.ProductDtos;
using PerfumeStore.Domain.Entities;

namespace PerfumeStore.Application.Dtos.BrandDtos
{
    public class GetByIdBrandDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Tagline { get; set; }
        public string Description { get; set; }
        public string LogoUrl { get; set; }
        public ICollection<ResultProductDto> products { get; set; } // Brendə aid məhsullar
    }
}
