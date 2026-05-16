namespace PerfumeStore.Application.Dtos.OrderItemDtos
{
    public class CreateOrderItemDto
    {
        public int ProductId { get; set; }
        public string Size { get; set; }  // Variant seçimi (məs: 50ml)
        public int Quantity { get; set; }
    }
}
