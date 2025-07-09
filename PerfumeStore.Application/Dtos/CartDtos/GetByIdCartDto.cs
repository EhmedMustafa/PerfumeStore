using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PerfumeStore.Application.Dtos.CartItemDtos;
using PerfumeStore.Application.Dtos.CustomerDtos;
using PerfumeStore.Domain.Entities;

namespace PerfumeStore.Application.Dtos.CartDtos
{
    public class GetByIdCartDto
    {
        public int CartId { get; set; }
        public decimal TotalAmount { get; set; }

        public DateTime Createddate { get; set; }
        public int CustomerId { get; set; }
        public ResultCustomerDto Customer { get; set; }
        public ICollection<ResultCartItemDto> CartItems { get; set; }
    }
}
