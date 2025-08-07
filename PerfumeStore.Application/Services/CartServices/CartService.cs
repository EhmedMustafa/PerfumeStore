using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using PerfumeStore.Application.Dtos.CartDtos;
using PerfumeStore.Application.Interfaces;
using PerfumeStore.Application.Interfaces.ICartRepository;
using PerfumeStore.Domain.Entities;

namespace PerfumeStore.Application.Services.CartServices
{
    public class CartService : ICartService
    {
        private readonly IGenericRepository<Cart> _genericRepository;
        private readonly IGenericRepository<Product> _product;
        private readonly IMapper _mapper;
        private readonly ICartRepository _cartRepository;
        public CartService(IGenericRepository<Cart> genericRepository, IGenericRepository<Product> product, IMapper mapper,ICartRepository cartRepository)
        {
            _genericRepository = genericRepository;
            _product = product;
            _mapper = mapper;
            _cartRepository = cartRepository;
        }

       


        public async Task AddCartAsync(CreateCartDto createCartDto)
        {

            var cart = new Cart
            {
                CustomerId = createCartDto.CustomerId,
                Createddate=DateTime.Now,
                CartItems= new List<CartItem>()
            };
            decimal totalamount = 0;
            foreach (var item in createCartDto.CartItems)
            {
                var product= await _product.GetByIdAsync(item.ProductVariantId);
                if (product == null) continue;

                var cartitem = new CartItem
                {
                    ProductVariantId = item.ProductVariantId,
                    Quantity = item.Quantity,
                    TotalPrice = product.ProductVariants.FirstOrDefault().OriginalPrice* item.Quantity
                };

                totalamount += cartitem.TotalPrice;
                cart.CartItems.Add(cartitem);
            }

            cart.TotalAmount= totalamount;

            await _genericRepository.AddAsync(cart);
            await _genericRepository.SaveChangesAsync();
            //await _product.SaveChangesAsync();  
        }

        public async Task DeleteCartAsync(int id)
        {
            var values = await _genericRepository.GetByIdAsync(id);
            if (values == null)
                throw new Exception("Səbət tapılmadı.");
            await _genericRepository.DeleteAsync(values);
            await _genericRepository.SaveChangesAsync();
        }

        public async Task<IEnumerable<ResultCartDto>> GetAllCartAsync()
        {
            var values= await _cartRepository.GetAllCartWithItemAsync();
            var map = _mapper.Map<IEnumerable<ResultCartDto>>(values);
            return map;
        }

        public async Task<GetByIdCartDto> GetByIdCartAsync(int id)
        {
            var value = await _cartRepository.GetCartByIdWithItemsAsync(id);
            var map= _mapper.Map<GetByIdCartDto>(value);
            return map;
        }

        public async Task<GetByIdCartDto> GetCartByUserIdWithItemsAsync(int userId)
        {
            var values = await _cartRepository.GetCartByUserIdWithItemsAsync(userId);
            var map = _mapper.Map<GetByIdCartDto>(values);
            return map;
        }
    }
}
