using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PerfumeStore.Application.Dtos.OrderItemDtos;

namespace PerfumeStore.Application.Dtos.OrderDto
{
    public class CreateOrderDto
    {
        public int UserId { get; set; }
        public List<OrderItemDto> OrderItems { get; set; } = new();
    }
}
