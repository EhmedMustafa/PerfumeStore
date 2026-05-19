using System;
using System.Collections.Generic;

namespace PerfumeStore.Domain.Entities
{
    public class Accessory
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; }
        public string ImageUrl { get; set; }

        // "flakon" və ya "giftbox"
        public string Type { get; set; } = "flakon";

        public bool IsAvailable { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<AccessoryVariant> Variants { get; set; } = new List<AccessoryVariant>();
    }
}
