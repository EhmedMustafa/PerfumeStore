using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PerfumeStore.Application.Dtos.ProductDtos;

namespace PerfumeStore.Application.Dtos.CartItemDtos
{
    public class ResultCartItemDto
    {
        public int CartItemId { get; set; }
        public int ProductId { get; set; }
        public ResultProductDto ProductName { get; set; }
        //public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
        // CartId lazımsızdır, çıxara bilərsən
    }
}
