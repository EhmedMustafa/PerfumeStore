using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PerfumeStore.Domain.Entities
{
    public class CartItem
    {
        public int CartItemId { get; set; }

        public int CartId { get; set; }
        public Cart Cart { get; set; }

        //public int ProductId { get; set; }
        //public Product Product { get; set; }
        public int ProductVariantId { get; set; }  // İndi ProductVariant-ə bağlı oluruq
        public ProductVariant ProductVariant { get; set; }
        public int Quantity { get; set; }

        // Opsional: qiyməti birbaşa hesablayıb saxlamaq üçün
        public decimal TotalPrice { get; set; }
    }
}
