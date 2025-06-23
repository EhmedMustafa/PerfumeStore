using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PerfumeStore.Domain.Entities;

namespace PerfumeStore.Application.Dtos.ProductDtos
{
    public class CreateProductDto
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; }
        public string Size { get; set; }
        public decimal CurrentPrice { get; set; }
        public decimal OriginalPrice { get; set; }
        public string ImageUrl { get; set; }
        public bool IsNew { get; set; }
        public bool IsBestseller { get; set; }
        public string Disclaimer { get; set; }  // Şəkil haqqında qeyd (məs: "Şəkil tanıtım xarakteri daşıyır")

        // Foreign Keys - Digər cədvəllərlə əlaqə üçün
        public int BrandId { get; set; }  // Brend ID-si
        public int FamilyId { get; set; }        // Ətri ailə ID-si
        public int CategoryId { get; set; }     // Cins ID-si



        // Navigation Properties - Digər cədvəllərlə əlaqəni təmin edən xüsusiyyətlər

        //public Brand Brand { get; set; }
       // public FragranceFamily Family { get; set; }
        //public Category Category { get; set; }

        // public ICollection<FragranceNote> notes { get; set; }

        public ICollection<ProductNote> ProductNotes { get; set; }
    }

}

