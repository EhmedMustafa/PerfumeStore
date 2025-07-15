using System;
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

        public async Task<List<Product>> GetAllWithNotesAsync()
        {
            return await _context.Products
               .Include(p => p.Brand)
               .Include(p => p.Family)
               .Include(p => p.Category)
               .Include(p => p.ProductNotes)
               .ThenInclude(pn => pn.FragranceNote).ToListAsync();
        }

        public async Task<List<Product>> GetBestsellerProductsAsync(int count)
        {
            return await _context.Products
                .Where(p=>p.IsBestseller)
                .OrderByDescending(p=>p.ProductId)
                .Take(count)
                .ToListAsync();
        }

        public async Task<List<Product>> GetDailyRotatedProductsAsync(int categoryId, int count)
        {
            var all = await _context.Products
                .Include(p => p.Category)
            .Where(p => p.CategoryId==categoryId)
            .OrderBy(p => p.ProductId)
            .ToListAsync();

            if (!all.Any()) return new List<Product>();

            int dayOfYear = DateTime.UtcNow.DayOfYear;
            int skip = (dayOfYear * count) % all.Count;

            var selected = all.Skip(skip).Take(count).ToList();

            if (selected.Count < count)
            {
                selected.AddRange(all.Take(count - selected.Count));
            }

            return selected;
        }

        public async Task<List<Product>> GetNewProductsAsync()
        {
            return await _context.Products
                .Where(p=>p.IsNew)
                .OrderByDescending (p=>p.ProductId)
                .ToListAsync();
        }

        public Task<List<Product>> GetProductByCategory(int categoryId)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Product>> GetProductByPriceFilter(decimal minprice, decimal maxprice)
        {
            throw new NotImplementedException();
        }

        public Task<List<Product>> GetProductBySearch(string search)
        {
            throw new NotImplementedException();
        }
        //public async Task<List<Product>> GetProductByCategory(int categoryId)
        //{
        //    return await _context.Products.Where(x => x.CategoryId == categoryId).ToListAsync();
        //}

        //public async Task<List<Product>> GetProductByPriceFilter(decimal minprice, decimal maxprice)
        //{
        //    return await _context.Products.Where(x => x.OriginalPrice >= minprice && x.OriginalPrice <= maxprice).ToListAsync();
        //}

        //public async Task<List<Product>> GetProductBySearch(string search)
        //{
        //    return await _context.Products.Where(x => x.Name.Contains(search) || x.Description.Contains(search)).ToListAsync();
        //}
    }
}
