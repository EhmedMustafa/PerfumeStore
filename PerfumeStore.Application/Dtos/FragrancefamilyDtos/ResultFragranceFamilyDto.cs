using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PerfumeStore.Domain.Entities;

namespace PerfumeStore.Application.Dtos.FragrancefamilyDtos
{
    public class ResultFragranceFamilyDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Product> products { get; set; } // Bu ailəyə aid məhsullar
    }
}
