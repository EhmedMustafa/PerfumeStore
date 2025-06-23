using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PerfumeStore.Domain.Entities;

namespace PerfumeStore.Application.Dtos.BrandDtos
{
    public class GetByIdBrandDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string LogoUrl { get; set; }
        public ICollection<Product> products { get; set; } // Brendə aid məhsullar
    }
}
