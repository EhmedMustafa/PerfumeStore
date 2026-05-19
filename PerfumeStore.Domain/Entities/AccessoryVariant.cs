namespace PerfumeStore.Domain.Entities
{
    public class AccessoryVariant
    {
        public int Id { get; set; }
        public int AccessoryId { get; set; }
        public Accessory Accessory { get; set; }

        public string Size { get; set; } = string.Empty;
        public decimal Price { get; set; }
    }
}
