using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PerfumeStore.Domain.Entities;

namespace PerfumeStore.Application.Dtos.BrandDtos
{
    public class UpdateBrandDto
    {
        public int Id { get; set; } // Hansı markanın yenilənəcəyini bilmək üçün
        public string Name { get; set; }
        public string LogoUrl { get; set; }
    } 
}
