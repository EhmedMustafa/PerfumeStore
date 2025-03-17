﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PerfumeStore.Application.Interfaces.IProductRepository;
using PerfumeStore.Domain.Entities;
using PerfumeStore.Infrastructure.Data;

namespace PerfumeStore.Infrastructure.Repositories.ProductRepository
{
    public class ProductsRepository : IProductRepository
    {
        private readonly AppDbContext _context;

        public ProductsRepository(AppDbContext Context)
        {
            _context = Context;
        }
        public async Task<List<Product>> GetProductByCategory(int categoryId)
        {
            return await _context.Products.Where(x => x.CategoryId == categoryId).ToListAsync();
        }

        public async Task<List<Product>> GetProductByPriceFilter(decimal minprice, decimal maxprice)
        {
            return await _context.Products.Where(x => x.Price >= minprice && x.Price <= maxprice).ToListAsync();
        }

        public async Task<List<Product>> GetProductBySearch(string search)
        {
            return await _context.Products.Where(x => x.Name.Contains(search) || x.Description.Contains(search)).ToListAsync();
        }
    }
}
