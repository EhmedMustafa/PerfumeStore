using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using PerfumeStore.Application.Dtos.OrderDto;
using PerfumeStore.Application.Dtos.OrderDtos;
using PerfumeStore.Application.Interfaces;
using PerfumeStore.Application.Interfaces.IOrderRepository;
using PerfumeStore.Application.Services.OrderServices;
using PerfumeStore.Domain.Entities;
using PerfumeStore.Domain.Enums;

namespace PerfumeStore.Infrastructure.Services
{
    public class OrderService : IOrderService
    {
        // Promo və çatdırılma qaydaları — config-dən gələ bilər
        private const string PROMO_CODE = "OMAR15";
        private const decimal PROMO_DISCOUNT = 0.15m;
        private const decimal FREE_SHIPPING_THRESHOLD = 50m;
        private const decimal SHIPPING_COST = 5m;

        private readonly IGenericRepository<Order> _orderRepository;
        private readonly IOrderRepository _orderReadRepository;
        private readonly IMapper _mapper;

        public OrderService(
            IGenericRepository<Order> orderRepository,
            IOrderRepository orderReadRepository,
            IMapper mapper)
        {
            _orderRepository = orderRepository;
            _orderReadRepository = orderReadRepository;
            _mapper = mapper;
        }

        public async Task<int> CreateOrderAsync(int userId, CreateOrderDto dto)
        {
            if (dto == null || dto.Items == null || !dto.Items.Any())
                throw new InvalidOperationException("Sifariş boş ola bilməz");

            if (string.IsNullOrWhiteSpace(dto.FirstName) ||
                string.IsNullOrWhiteSpace(dto.LastName) ||
                string.IsNullOrWhiteSpace(dto.Phone) ||
                string.IsNullOrWhiteSpace(dto.Address) ||
                string.IsNullOrWhiteSpace(dto.City))
            {
                throw new InvalidOperationException("Çatdırılma məlumatları natamamdır");
            }

            // Bütün lazımi məhsul və variantları bir dəfəyə yüklə
            var productIds = dto.Items.Select(i => i.ProductId).Distinct().ToList();
            var products = await _orderReadRepository.GetProductsWithVariantsAsync(productIds);

            var orderItems = new List<OrderItem>();
            decimal subtotal = 0m;

            foreach (var item in dto.Items)
            {
                if (item.Quantity <= 0) continue;

                var product = products.FirstOrDefault(p => p.ProductId == item.ProductId);
                if (product == null) continue;

                // Variant seçimi: Size verilibsə uyğun variant, yoxdursa ilk variant
                ProductVariant variant = null;
                if (!string.IsNullOrWhiteSpace(item.Size))
                {
                    variant = product.ProductVariants?.FirstOrDefault(v => v.Size == item.Size);
                }
                variant ??= product.ProductVariants?.FirstOrDefault();
                if (variant == null) continue;

                var unitPrice = variant.CurrentPrice;
                var lineTotal = unitPrice * item.Quantity;
                subtotal += lineTotal;

                orderItems.Add(new OrderItem
                {
                    ProductId = product.ProductId,
                    ProductName = product.Name,
                    BrandName = product.Brand?.Name,
                    Size = variant.Size,
                    Quantity = item.Quantity,
                    UnitPrice = unitPrice,
                    TotalPrice = lineTotal
                });
            }

            if (!orderItems.Any())
                throw new InvalidOperationException("Heç bir keçərli məhsul tapılmadı");

            // Çatdırılma
            decimal shippingCost = subtotal >= FREE_SHIPPING_THRESHOLD ? 0m : SHIPPING_COST;

            // Hədiyyə qablaşdırma
            decimal giftCost = dto.GiftOption switch
            {
                "gift-box" => 5m,
                "premium-box" => 10m,
                _ => 0m
            };

            // Promo kod (server-side validasiya — client-dəkinə güvənmirik)
            decimal discountAmount = 0m;
            if (!string.IsNullOrWhiteSpace(dto.PromoCode) &&
                string.Equals(dto.PromoCode.Trim(), PROMO_CODE, StringComparison.OrdinalIgnoreCase))
            {
                discountAmount = Math.Round(subtotal * PROMO_DISCOUNT, 2);
            }

            decimal totalAmount = subtotal + shippingCost + giftCost - discountAmount;

            var order = new Order
            {
                UserId = userId,
                OrderDate = DateTime.UtcNow,
                Status = OrderStatus.Pending,

                FirstName = dto.FirstName?.Trim(),
                LastName = dto.LastName?.Trim(),
                Email = dto.Email?.Trim(),
                Phone = dto.Phone?.Trim(),
                Address = dto.Address?.Trim(),
                City = dto.City?.Trim(),
                ZipCode = dto.ZipCode?.Trim(),
                Note = dto.Note?.Trim(),

                PaymentMethod = string.IsNullOrWhiteSpace(dto.PaymentMethod) ? "cash" : dto.PaymentMethod,
                GiftOption = string.IsNullOrWhiteSpace(dto.GiftOption) ? "none" : dto.GiftOption,
                GiftMessage = dto.GiftMessage?.Trim(),
                PromoCode = dto.PromoCode?.Trim(),

                Subtotal = subtotal,
                ShippingCost = shippingCost,
                DiscountAmount = discountAmount,
                GiftCost = giftCost,
                TotalAmount = totalAmount,

                UpdatedAt = DateTime.UtcNow,
                OrderItems = orderItems
            };

            await _orderRepository.AddAsync(order);
            await _orderRepository.SaveChangesAsync();

            return order.OrderId;
        }

        public async Task<IEnumerable<ResultOrderDto>> GetAllOrderAsync()
        {
            var values = await _orderReadRepository.GetOrderWithOrderItems();
            return _mapper.Map<IEnumerable<ResultOrderDto>>(values);
        }

        public async Task<GetByIdOrderDto> GetOrderByIdAsync(int id)
        {
            var values = await _orderReadRepository.GetOrderByIdWithOrderItems(id);
            return _mapper.Map<GetByIdOrderDto>(values);
        }

        public async Task<IEnumerable<ResultOrderDto>> GetOrdersByUserIdAsync(int userId)
        {
            var all = await _orderReadRepository.GetOrderWithOrderItems();
            var mine = all.Where(o => o.UserId == userId).ToList();
            return _mapper.Map<IEnumerable<ResultOrderDto>>(mine);
        }

        public async Task UpdateOrderAsync(UpdateOrderDto updateOrderDto)
        {
            var order = await _orderRepository.GetByIdAsync(updateOrderDto.OrderId);
            if (order == null) return;

            order.Status = updateOrderDto.Status;
            if (!string.IsNullOrWhiteSpace(updateOrderDto.Note))
                order.Note = updateOrderDto.Note;
            order.UpdatedAt = DateTime.UtcNow;

            await _orderRepository.UpdateAsync(order);
            await _orderRepository.SaveChangesAsync();
        }

        public async Task<bool> UpdateOrderStatusAsync(int orderId, OrderStatus status)
        {
            var order = await _orderRepository.GetByIdAsync(orderId);
            if (order == null) return false;

            order.Status = status;
            order.UpdatedAt = DateTime.UtcNow;
            await _orderRepository.UpdateAsync(order);
            await _orderRepository.SaveChangesAsync();
            return true;
        }

        public async Task DeleteOrderAsync(int id)
        {
            var values = await _orderRepository.GetByIdAsync(id);
            if (values == null) return;
            await _orderRepository.DeleteAsync(values);
            await _orderRepository.SaveChangesAsync();
        }
    }
}
