using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PerfumeStore.Domain.Entities;

namespace PerfumeStore.Application.Dtos.BrandDtos
{
    public class CreateBrandDto
    {
       
        public string Name { get; set; }               // Brendin adı
        public string Tagline { get; set; }
        public string Description { get; set; }
        public string LogoUrl { get; set; }            // Brend loqosunun URL-i        
    }
}
