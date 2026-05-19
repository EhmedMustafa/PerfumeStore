using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PerfumeStore.Domain.Entities
{
    public class Product
    {
        public int ProductId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; }
        //public string Size { get; set; }
        //public decimal CurrentPrice { get; set; }
        //public decimal OriginalPrice { get; set; }
        public string ImageUrl { get; set; }
        public bool IsNew { get; set; }
        public bool IsBestseller { get; set; }
        public string Disclaimer { get; set; }  // Şəkil haqqında qeyd (məs: "Şəkil tanıtım xarakteri daşıyır")

        // Quiz scoring üçün — admin tərəfindən seçilir
        // Season: "yay" | "qış" | "yaz-payız" | "hər"
        public string Season { get; set; }

        // Occasion: "gündəlik" | "gecə" | "idman" | "hər"
        public string Occasion { get; set; }


        // Foreign Keys - Digər cədvəllərlə əlaqə üçün
        public int BrandId { get; set; }  // Brend ID-si
        public int FamilyId { get; set; }        // Ətri ailə ID-si
        public int CategoryId { get; set; }     // Cins ID-si



        // Navigation Properties - Digər cədvəllərlə əlaqəni təmin edən xüsusiyyətlər

        public Brand Brand { get; set; }
        public FragranceFamily Family { get; set; }
        public Category Category { get; set; }

        // public ICollection<FragranceNote> notes { get; set; }
        public IEnumerable<ProductVariant> ProductVariants { get; set; }
        public ICollection<ProductNote> ProductNotes { get; set; }
    }
}
