using System.Collections.Generic;
using PerfumeStore.Application.Dtos.OrderItemDtos;

namespace PerfumeStore.Application.Dtos.OrderDto
{
    public class CreateOrderDto
    {
        // UserId artıq client-dən GƏLMİR — token-dən götürülür.

        // Çatdırılma məlumatları
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }
        public string Note { get; set; }

        public string PaymentMethod { get; set; } = "cash";
        public string GiftOption { get; set; } = "none";
        public string GiftMessage { get; set; }
        public string PromoCode { get; set; }

        public ICollection<CreateOrderItemDto> Items { get; set; } = new List<CreateOrderItemDto>();

        // Frontend uyğunluğu üçün köhnə ad da dəstəklənir
        public ICollection<CreateOrderItemDto> createOrderItemDtos
        {
            get => Items;
            set => Items = value;
        }
    }
}
