using System;
using PerfumeStore.Domain.Enums;

namespace PerfumeStore.Application.Dtos.OrderDtos
{
    public class UpdateOrderDto
    {
        public int OrderId { get; set; }
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
        public string Note { get; set; }
    }
}
