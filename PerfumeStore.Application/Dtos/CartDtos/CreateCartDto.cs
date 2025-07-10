using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PerfumeStore.Application.Dtos.CartItemDtos;
using PerfumeStore.Domain.Entities;

namespace PerfumeStore.Application.Dtos.CartDtos
{
    public class CreateCartDto
    {
        public int? CustomerId { get; set; }

        public ICollection<CreateCartItemDto> CartItems { get; set; } = new List<CreateCartItemDto>();
    }
}
