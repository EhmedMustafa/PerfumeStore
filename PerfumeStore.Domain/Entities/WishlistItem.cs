using System;
using PerfumeStore.Domain.Entities.Identity;

namespace PerfumeStore.Domain.Entities
{
    public class WishlistItem
    {
        public int Id { get; set; }
        public int AppUserId { get; set; }
        public AppUser AppUser { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
