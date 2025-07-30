using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PerfumeStore.Application.Dtos.ProductVariantDtos
{
   public class GetByIdProductVariantDto
    {

        public int Id { get; set; }
        public string Size { get; set; }
        public decimal CurrentPrice { get; set; }
        public decimal OriginalPrice { get; set; }
       // public bool InStock { get; set; }

        public int ProductId { get; set; }
        public string ProductName { get; set; }
    }
}
