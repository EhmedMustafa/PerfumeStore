using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using PerfumeStore.Application.Dtos.CartItemDtos;
using PerfumeStore.Application.Interfaces;
using PerfumeStore.Application.Interfaces.ICartRepository;
using PerfumeStore.Application.Services.CartItemItemServices;
using PerfumeStore.Domain.Entities;

namespace PerfumeStore.Application.Services.CartItemServices
{
    public class CartItemService : ICartItemService
    {
        private readonly IGenericRepository<CartItem> _genericRepository;
        private readonly IMapper _mapper;
        private readonly IGenericRepository<Product> _product;
        private readonly IGenericRepository<Cart> _cart;
        private readonly ICartRepository _cartRepository;

        public CartItemService(IGenericRepository<CartItem> genericRepository, IMapper mapper, IGenericRepository<Cart> cart)
        {
            _genericRepository = genericRepository;
            _mapper = mapper;
            _cart = cart;
        }

        public async Task AddCartItemAsync(int cartId, CreateCartItemDto createCartItemDto)
        {
            
            var cart = await _cartRepository.GetCartWithItemsAsync(cartId);
            if (cart == null) throw new Exception("Səbət tapılmadı.");

            var product = await _product.GetByIdAsync(createCartItemDto.ProductId);
            if (product == null) throw new Exception("Məhsul tapılmadı.");

            if(createCartItemDto.Quantity<=0) throw new Exception("Məhsulun sayı sıfır və ya mənfi ola bilməz.");

            var existingItem= cart.CartItems.FirstOrDefault(ci=>ci.ProductId==createCartItemDto.ProductId);

            if (existingItem != null)
            {
                existingItem.Quantity += createCartItemDto.Quantity;
                existingItem.TotalPrice = existingItem.Quantity * product.OriginalPrice;
            }

            else 
            {
                var cartitem = new CartItem
                {
                    CartId = cartId,
                    ProductId = createCartItemDto.ProductId,
                    Quantity = createCartItemDto.Quantity,
                    TotalPrice = createCartItemDto.Quantity * product.OriginalPrice,
                };
                await _genericRepository.AddAsync(cartitem);
            }

            cart.TotalAmount = cart.CartItems.Sum(ci => ci.TotalPrice);
            await _genericRepository.SaveChangesAsync();
            await _cart.SaveChangesAsync();
        }

        public async Task DeleteCartItemAsync(int id)
        {
            var values= await _genericRepository.GetByIdAsync(id);
            if (values == null)
                throw new Exception("Səbət elementi tapılmadı.");
            await _genericRepository.DeleteAsync(values);
            await _genericRepository.SaveChangesAsync();
        }

        public async Task<IEnumerable<ResultCartItemDto>> GetAllCartItemAsync()
        {
            var values= await _genericRepository.GetAllAsync();
            var map = _mapper.Map<IEnumerable<ResultCartItemDto>>(values);
            return map;
        }

        public async Task<GetByIdCartItemDto> GetByIdCartItemAsync(int id)
        {
            var value= await _genericRepository.GetByIdAsync(id);
            var map = _mapper.Map<GetByIdCartItemDto>(value);
            return map;
        }

        public async Task UpdateCartItemAsync(UpdateCartItemDto updateCartItemDto)
        {
            var cartItem = await _genericRepository.GetByIdAsync(updateCartItemDto.CartItemId);
            if (cartItem == null)
                throw new Exception("Səbət məhsulu tapılmadı.");

            if( updateCartItemDto.Quantity<=0)
                throw new Exception("Məhsul miqdarı sıfır və ya mənfi ola bilməz.");

            var product = await _product.GetByIdAsync(cartItem.ProductId);
            if (product == null)
                throw new Exception("Məhsul mövcud deyil.");

            cartItem.Quantity=updateCartItemDto.Quantity;
            cartItem.TotalPrice=updateCartItemDto.Quantity*product.OriginalPrice;

            await _genericRepository.UpdateAsync(cartItem);

            var cart = await _cart.GetByIdAsync(cartItem.CartId);
            if (cart != null)
            {
                cart.TotalAmount = cart.CartItems.Sum(x => x.TotalPrice);
                await _cart.UpdateAsync(cart);
            }

            await _genericRepository.SaveChangesAsync();
            await _cart.SaveChangesAsync();

        }
    }
}
