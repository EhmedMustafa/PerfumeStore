using System;

namespace PerfumeStore.Domain.Entities
{
    public class Promo
    {
        public int Id { get; set; }

        // Kod — UPPERCASE, unique
        public string Code { get; set; } = string.Empty;

        // Endirim faizi — 0-100 (məsələn 15 = 15%)
        public int DiscountPercent { get; set; }

        // Müddət (null = istənilən vaxt)
        public DateTime? ValidFrom { get; set; }
        public DateTime? ValidUntil { get; set; }

        // İstifadə limiti (null = limitsiz)
        public int? UsageLimit { get; set; }
        public int UsedCount { get; set; } = 0;

        // Minimum sifariş məbləği (null = limit yox)
        public decimal? MinOrderAmount { get; set; }

        // Yalnız ilk sifariş üçün
        public bool FirstOrderOnly { get; set; } = false;

        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Vacib: opsional açıqlama (admin üçün)
        public string Description { get; set; } = string.Empty;
    }
}
