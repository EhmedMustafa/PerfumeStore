using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PerfumeStore.Application.Dtos.CartDtos
{
   public class AddToCartRequest
    {
        public int ProductId { get; set; }
        public List<CartItemDto> Items { get; set; }
    }
}
