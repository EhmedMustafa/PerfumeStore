using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.VisualBasic;
using PerfumeStore.Application.Dtos.CartItemDtos;
using PerfumeStore.Application.Interfaces;
using PerfumeStore.Application.Interfaces.ICartRepository;
using PerfumeStore.Application.Interfaces.IProductVariant;
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
        private readonly IGenericRepository<ProductVariant> _productVariant;
        private readonly IProductVariantRepository _productVariant1;


        public CartItemService(IGenericRepository<CartItem> genericRepository, IMapper mapper, IGenericRepository<Cart> cart, ICartRepository cartRepository, IGenericRepository<Product> product, IGenericRepository<ProductVariant> productVariant, IProductVariantRepository productVariant1)
        {
            _genericRepository = genericRepository;
            _mapper = mapper;
            _cart = cart;
            _cartRepository = cartRepository;
            _product = product;
            _productVariant = productVariant;
            _productVariant1 = productVariant1;
        }

        public async Task AddCartItemAsync(int cartId, CreateCartItemDto createCartItemDto)
        {

            var cart = await _cartRepository.GetCartByUserIdWithItemsAsync(cartId); 

            if (cart == null)
            {
                cart = new Cart
                {
                    CustomerId = cartId,
                    Createddate = DateTime.Now,
                    CartItems = new List<CartItem>()
                };

                await _cartRepository.AddAsync(cart);
                await _cart.SaveChangesAsync();
            }

            var variant = await _productVariant1.GetVariantWithDetailsByIdAsync(createCartItemDto.ProductVariantId);
            if (variant == null)
                throw new Exception("Məhsul variantı tapılmadı."); // variant null-dursa, burda dayanacaq

            if (createCartItemDto.Quantity <= 0)
                throw new Exception("Məhsulun sayı sıfır və ya mənfi ola bilməz.");

            // Əgər CartItems null-dursa, boş list ver
            if (cart.CartItems == null)
                cart.CartItems = new List<CartItem>();

            var existingItem = cart.CartItems.FirstOrDefault(ci => ci.ProductVariantId == createCartItemDto.ProductVariantId);

            if (existingItem != null)
            {
                existingItem.Quantity += createCartItemDto.Quantity;
                existingItem.TotalPrice = existingItem.Quantity * variant.CurrentPrice;
                
            }
            else
            {
                var cartItem = new CartItem
                {
                    CartId = cart.CartId,
                    ProductVariantId = createCartItemDto.ProductVariantId,
                    Quantity = createCartItemDto.Quantity,
                    TotalPrice = createCartItemDto.Quantity * variant.CurrentPrice
                };

                cart.CartItems.Add(cartItem); // Bu da vacibdir
                await _genericRepository.AddAsync(cartItem);
            }

            // Yenidən yoxla ki, cart.CartItems null deyil
            cart.TotalAmount = cart.CartItems?.Sum(ci => ci.TotalPrice) ?? 0;

            await _genericRepository.SaveChangesAsync();
           // await _cart.SaveChangesAsync();
        }



        public async Task <bool>DeleteCartItemAsync(int id)
        {
            var values = await _genericRepository.GetByIdAsync(id);
            if (values == null)
                throw new Exception("Səbət elementi tapılmadı.");

            var cart = await _cartRepository.GetCartByIdWithItemsAsync(values.CartId);


            await _genericRepository.DeleteAsync(values);

            cart.TotalAmount = cart.CartItems.Where(ci => ci.CartItemId != id).Sum(ci => ci.TotalPrice);
            await _genericRepository.SaveChangesAsync();
            return true;
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

        public async Task<bool> UpdateCartItemAsync(UpdateCartItemDto updateCartItemDto)
        {
            var cartItem = await _genericRepository.GetByIdAsync(updateCartItemDto.CartItemId);
            if (cartItem == null)
                throw new Exception("Səbət məhsulu tapılmadı.");

            if( updateCartItemDto.Quantity<=0)
                throw new Exception("Məhsul miqdarı sıfır və ya mənfi ola bilməz.");

            var product = await _product.GetByIdAsync(cartItem.ProductVariantId);
            if (product == null)
                throw new Exception("Məhsul mövcud deyil.");

            cartItem.Quantity=updateCartItemDto.Quantity;
            cartItem.TotalPrice=updateCartItemDto.Quantity*product.ProductVariants.FirstOrDefault().OriginalPrice;

            await _genericRepository.UpdateAsync(cartItem);

            var cart = await _cartRepository.GetCartByIdWithItemsAsync(cartItem.CartId);
            if (cart != null)
            {
                cart.TotalAmount = cart.CartItems.Sum(x => x.TotalPrice);
                await _cart.UpdateAsync(cart);
            }

            await _genericRepository.SaveChangesAsync();
            await _cart.SaveChangesAsync();
            return true;
        }
    }
}
