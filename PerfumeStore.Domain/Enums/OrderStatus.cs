using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PerfumeStore.Domain.Enums
{
    public enum OrderStatus
    {
        Pending,     // Gözləmədə
        Processing,  // Hazırlanır
        Shipped,     // Göndərildi
        Delivered,   // Çatdırıldı
        Canceled     // Ləğv edildi
    }
}
