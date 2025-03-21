﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PerfumeStore.Domain.Entities;

namespace PerfumeStore.Application.Interfaces.IProductRepository
{
    public interface IProductRepository
    {
        Task<List<Product>> GetProductByCategory(int categoryId);
        Task<List<Product>> GetProductByPriceFilter(decimal minprice, decimal maxprice);
        Task<List<Product>> GetProductBySearch(string search);
    }
}
