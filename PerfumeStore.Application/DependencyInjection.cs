using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using PerfumeStore.Application.Services;
using PerfumeStore.Application.Services.BrandServices;
using PerfumeStore.Application.Services.CartItemItemServices;
using PerfumeStore.Application.Services.CartItemServices;
using PerfumeStore.Application.Services.CartServices;
using PerfumeStore.Application.Services.CategoryServices;
using PerfumeStore.Application.Services.CustomerServices;
using PerfumeStore.Application.Services.FragranceFamilyService;
using PerfumeStore.Application.Services.FragranceNoteServices;
using PerfumeStore.Application.Services.OrderItemServices;
using PerfumeStore.Application.Services.OrderServices;
using PerfumeStore.Application.Services.ProductServices;
using PerfumeStore.Domain.Entities;
using PerfumeStore.Infrastructure.Services;



namespace PerfumeStore.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
           // services.AddSingleton<IProductService, ProductService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<ICustomerService, CustomerService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IOrderItemService, OrderItemService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IBrandService, BrandService>();
            services.AddScoped<IFragranceFamilyService, FragranceFamilyService>();
            services.AddScoped<IFragranceNoteService,FragranceNoteService>();
            services.AddScoped<ICartService, CartService>();
            services.AddScoped<ICartItemService, CartItemService>();

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            return services;
        }
    }
}
