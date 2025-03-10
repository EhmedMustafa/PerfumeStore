using PerfumeStore.Application.Interfaces;
using PerfumeStore.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PerfumeStore.Infrastructure.Services
{
    public class ProductService : IProductService
    {
       private readonly IProductService _productService;
    }


}
