using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using PerfumeStore.Application.Services;
using PerfumeStore.Domain.Entities;


namespace PerfumeStore.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            //services.AddScoped<IProductService,ProductService>();


            return services;
        }
    }
}
