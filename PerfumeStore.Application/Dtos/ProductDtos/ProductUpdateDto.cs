using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PerfumeStore.Application.Dtos.ProductDtos
{
    public class ProductUpdateDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string Size { get; set; } = string.Empty;
        public string Brand { get; set; } = string.Empty;
        public string TopNotes { get; set; } = string.Empty;
        public string MiddleNotes { get; set; } = string.Empty;
        public string BaseNotes { get; set; } = string.Empty;
        public int StockQuantity { get; set; }
        public decimal Discount { get; set; }
        public string Description { get; set; } = string.Empty;

    }
}
