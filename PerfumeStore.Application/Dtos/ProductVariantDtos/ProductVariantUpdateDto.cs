using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PerfumeStore.Application.Dtos.ProductVariantDtos
{
    public class ProductVariantUpdateDto
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string Size { get; set; }
        public decimal CurrentPrice { get; set; }
        public decimal OriginalPrice { get; set; }
        //public string? ImageUrl { get; set; }
       // public int StockQuantity { get; set; }
    }
}
