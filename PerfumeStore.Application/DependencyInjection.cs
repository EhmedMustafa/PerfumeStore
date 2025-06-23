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

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            return services;
        }
    }
}
