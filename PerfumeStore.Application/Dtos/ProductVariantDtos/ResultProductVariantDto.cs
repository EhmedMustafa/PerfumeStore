using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PerfumeStore.Application.Dtos.ProductVariantDtos
{
    public class ResultProductVariantDto
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public string Size { get; set; }           // Məsələn: "50ml", "100ml"

        public decimal CurrentPrice { get; set; }  // Endirimli qiymət

        public decimal OriginalPrice { get; set; } // Əvvəlki qiymət (əgər varsa)

        //  public bool InStock { get; set; }          // Stokda olub-olmaması
        public string ProductImageUrl { get; set; } // Product.ImageUrl gələcək
        public int ProductId { get; set; }         // Əlaqəli Product ID
    }
}
