using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using PerfumeStore.Application.Services;
using PerfumeStore.Application.Services.CategoryServices;
using PerfumeStore.Application.Services.CustomerServices;
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

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            return services;
        }
    }
}
