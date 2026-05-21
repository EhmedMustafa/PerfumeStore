using System.Collections.Generic;
using PerfumeStore.Application.Dtos.ProductVariantDtos;
using PerfumeStore.Domain.Entities;

namespace PerfumeStore.Application.Dtos.ProductDtos
{
    public class UpdateProductDto
    {
        public int ProductId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public bool IsNew { get; set; }
        public bool IsBestseller { get; set; }
        public string Disclaimer { get; set; }
        public string Season { get; set; }
        public string Occasion { get; set; }
        public bool IsAccessory { get; set; }
        public bool IsMonthlyFeatured { get; set; }
        public List<string> GalleryImages { get; set; } = new List<string>();

        // Foreign Keys
        public int BrandId { get; set; }
        public int FamilyId { get; set; }
        public int CategoryId { get; set; }

        public IEnumerable<ProductVariantCreateDto> ProductVariants { get; set; }
        public ICollection<ProductNote> ProductNotes { get; set; }
    }
}
