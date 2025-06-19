using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PerfumeStore.Domain.Entities
{
    public class OrderItem
    {
        public int OrderItemId { get; set; }
        public int OrderId { get; set; }//Bu məhsul hansı sifarişə aid olduğunu göstərir.
        public Order Order { get; set; }
        public int ProductId { get; set; }//Sifarişdə hansı məhsulun olduğunu göstərir.
        public Product Product { get; set; }
        public int Quantity { get; set; }//Məhsulun sifarişdə neçə ədəd olduğunu göstərir.
        public decimal TotalPrice { get; set; } //Məhsulun sifariş zamanı olan qiyməti qeyd edilir.
    }
}
