using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PerfumeStore.Domain.Entities
{
    public class ProductVariant
    {
        public int Id { get; set; }
        public int ProductId { get; set; }

        public string Size { get; set; }  // Məs: 30ml, 50ml, 100ml
        public decimal CurrentPrice { get; set; }
        public decimal OriginalPrice { get; set; }

       // public string? ImageUrl { get; set; }  // Əgər ölçü üzrə ayrıca şəkil varsa

        public Product Product { get; set; }
    }

}
