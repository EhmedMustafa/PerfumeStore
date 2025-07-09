using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PerfumeStore.Application.Interfaces;
using PerfumeStore.Application.Interfaces.ICartRepository;
using PerfumeStore.Application.Interfaces.IOrderItemRepository;
using PerfumeStore.Application.Interfaces.IOrderRepository;
using PerfumeStore.Application.Interfaces.IProductRepository;
using PerfumeStore.Infrastructure.Data;
using PerfumeStore.Infrastructure.Repositories;
using PerfumeStore.Infrastructure.Repositories.CartRepository;
using PerfumeStore.Infrastructure.Repositories.OrderItemRepository;
using PerfumeStore.Infrastructure.Repositories.OrderRepository;
using PerfumeStore.Infrastructure.Repositories.ProductRepository;

namespace PerfumeStore.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration) 
        {
            services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));


            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped(typeof(IProductRepository), typeof(ProductsRepository));
            services.AddScoped(typeof(IFragranceNoteRepository), typeof(FragranceNoteRepository));
            services.AddScoped(typeof(IOrderRepository), typeof(OrderRepository));
            services.AddScoped(typeof(IOrderItemRepository), typeof(OrderItemRepository));
            services.AddScoped(typeof(ICartRepository), typeof(CartRepository));
                
            return services;
        }
    }
}
