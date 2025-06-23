using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PerfumeStore.Domain.Entities.Identity;
using PerfumeStore.Domain.Entities;
using PerfumeStore.Domain.Enums;

namespace PerfumeStore.Application.Dtos.OrderDtos
{
   public class GetByIdOrderDto
    {
        public int OrderId { get; set; }
        public int UserId { get; set; }
        public AppUser User { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;
        public decimal TotalAmount { get; set; }
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}
