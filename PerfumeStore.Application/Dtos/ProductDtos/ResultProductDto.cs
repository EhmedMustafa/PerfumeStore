using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PerfumeStore.Application.Dtos.ProductVariantDtos;
using PerfumeStore.Domain.Entities;

namespace PerfumeStore.Application.Dtos.ProductDtos
{
    public class ResultProductDto
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

        public int BrandId { get; set; }
        public string BrandName { get; set; }

        public int FamilyId { get; set; }
        public string FamilyName { get; set; }

        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public List<ProductVariantCreateDto> ProductVariants { get; set; }
        public ICollection<ProductNoteDto> ProductNotes { get; set; }

        //public List<int> FragranceNoteIds => ProductNotes?.Select(p => p.FragranceNoteId).ToList() ?? new List<int>();

    }
}
