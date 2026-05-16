using System;
using System.Collections.Generic;
using PerfumeStore.Application.Dtos.OrderItemDtos;
using PerfumeStore.Domain.Enums;

namespace PerfumeStore.Application.Dtos.OrderDtos
{
    public class GetByIdOrderDto
    {
        public int OrderId { get; set; }
        public int UserId { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }
        public string Note { get; set; }

        public string PaymentMethod { get; set; }
        public string GiftOption { get; set; }
        public string GiftMessage { get; set; }
        public string PromoCode { get; set; }

        public decimal Subtotal { get; set; }
        public decimal ShippingCost { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal GiftCost { get; set; }
        public decimal TotalAmount { get; set; }

        public OrderStatus Status { get; set; } = OrderStatus.Pending;

        public ICollection<ResultOrderItemDto> OrderItems { get; set; } = new List<ResultOrderItemDto>();
    }
}
