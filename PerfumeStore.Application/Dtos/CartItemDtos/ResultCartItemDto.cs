using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PerfumeStore.Application.Dtos.ProductDtos;
using PerfumeStore.Application.Dtos.ProductVariantDtos;

namespace PerfumeStore.Application.Dtos.CartItemDtos
{
    public class ResultCartItemDto
    {
        public int CartItemId { get; set; }
        public int ProductId { get; set; }
        public ResultProductDto ProductName { get; set; }
        public ResultProductVariantDto ProductVariant { get; set; }  // Variant məlumatları üçün
        //public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
        // CartId lazımsızdır, çıxara bilərsən
    }
}
