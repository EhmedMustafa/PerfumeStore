using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PerfumeStore.Application.Dtos.CartItemDtos
{
    public class GetByIdCartItemDto
    {
        public int CartItemId { get; set; }

        public int ProductId { get; set; }
        public int ProductVariantId { get; set; }
        public string ProductName { get; set; }  // Məhsul adı rahatlıq üçün
                                                 // Variantın ölçüsü, qiymət və s.
        public string Size { get; set; }
        public decimal CurrentPrice { get; set; }
        public decimal OriginalPrice { get; set; }
        public int Quantity { get; set; }
        public string ImageUrl { get; set; }
        public decimal UnitPrice { get; set; }   // Məhsulun vahid qiyməti
        public decimal TotalPrice { get; set; }  // Quantity * UnitPrice

        // Optional olaraq CartId da əlavə edə bilərsən:
        // public int CartId { get; set; }
    }
}
