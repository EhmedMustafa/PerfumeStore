using System;

namespace PerfumeStore.Domain.Entities
{
    public class HomeBanner
    {
        public int Id { get; set; }

        // Görünüş
        public string Title { get; set; } = string.Empty;        // Böyük başlıq
        public string Subtitle { get; set; } = string.Empty;     // Qısa açıqlama
        public string ImageUrl { get; set; } = string.Empty;     // Arxa fon şəkli

        // CTA düyməsi
        public string CtaText { get; set; } = string.Empty;      // "İndi al"
        public string CtaLink { get; set; } = string.Empty;      // "/shop" və ya "/product/42"

        // Tema: 'dark' (ağ mətn) və ya 'light' (qara mətn)
        public string Theme { get; set; } = "dark";

        // İdarə
        public bool IsActive { get; set; } = true;
        public int SortOrder { get; set; } = 0;                   // Slider sırası
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
