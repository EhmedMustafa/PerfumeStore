using System;
using System.Collections.Generic;
using PerfumeStore.Domain.Entities.Identity;
using PerfumeStore.Domain.Enums;

namespace PerfumeStore.Domain.Entities
{
    public class Order
    {
        public int OrderId { get; set; }
        public int UserId { get; set; }
        public AppUser User { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;

        // Müştəri / çatdırılma məlumatları (sifariş anındakı snapshot)
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }
        public string Note { get; set; }

        // Ödəniş və əlavə
        public string PaymentMethod { get; set; } = "cash";   // 'cash' | 'transfer'
        public string GiftOption { get; set; } = "none";      // 'none' | 'gift-box' | 'premium-box'
        public string GiftMessage { get; set; }
        public string PromoCode { get; set; }

        // Məbləğ hesablamaları (server-side hesablanır)
        public decimal Subtotal { get; set; }
        public decimal ShippingCost { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal GiftCost { get; set; }
        public decimal TotalAmount { get; set; }

        public OrderStatus Status { get; set; } = OrderStatus.Pending;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}
