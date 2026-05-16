namespace PerfumeStore.Domain.Entities
{
    public class OrderItem
    {
        public int OrderItemId { get; set; }
        public int OrderId { get; set; }
        public Order Order { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }

        // Sifariş anındakı snapshot — məhsul/qiymət sonradan dəyişsə də bunlar qalır
        public string ProductName { get; set; }
        public string BrandName { get; set; }
        public string Size { get; set; }

        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
