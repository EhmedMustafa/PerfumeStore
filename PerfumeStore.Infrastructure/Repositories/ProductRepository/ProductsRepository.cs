using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using PerfumeStore.Application.Dtos.ProductDtos;
using PerfumeStore.Application.Interfaces.IProductRepository;
using PerfumeStore.Domain.Entities;
using PerfumeStore.Infrastructure.Data;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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
                .Include(p=>p.ProductVariants)
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



        public async Task<PaginatedResult<Product>> GetPagedProductsAsync(List<int> categoryIds, List<int> brandId, List<int> fragranceFamilyId, List<int> fragranceNoteId, int page, int pageSize, int? minPrice, int? maxPrice)
        {
            

            var query = _context.Products
                    .Include(p => p.Brand)
                    .Include(p => p.Category)
                    .Include(p => p.Family)
                    .Include(p=>p.ProductVariants)
                    .Include(p => p.ProductNotes)
                    
                    .ThenInclude(pn=>pn.FragranceNote)
                    .AsQueryable();

            if (categoryIds != null && categoryIds.Any())
                query = query.Where(p => categoryIds.Contains(p.CategoryId));

            if (brandId != null && brandId.Any())
                query = query.Where(b => brandId.Contains(b.BrandId));

            if (fragranceFamilyId != null && fragranceFamilyId.Any())
                query = query.Where(f => fragranceFamilyId.Contains(f.FamilyId));

            if(fragranceNoteId != null && fragranceNoteId.Any())
                query = query.Where(p => p.ProductNotes.Any(pn => fragranceNoteId.Contains(pn.FragranceNoteId)));


            if (minPrice.HasValue)
            {
                query = query.Where(p =>
                    p.ProductVariants.Any(v => v.CurrentPrice >= minPrice.Value));
            }

            if (maxPrice.HasValue)
            {
                query = query.Where(p =>
                    p.ProductVariants.Any(v => v.CurrentPrice <= maxPrice.Value));
            }

            var totalCount = await query.CountAsync();

                var items = await query
                    .OrderByDescending(p => p.ProductId)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                return new PaginatedResult<Product>
                {
                    Items = items,
                    TotalCount = totalCount,
                    PageSize = pageSize,
                    CurrentPage = page
                };
        }

        //lazim olacaq istifade olumur
        public async Task<PaginatedResult<Product>> GetPagedProductsByCategoryAsync(int categoryId, int page, int pageSize)
        {
            var query = _context.Products
                      .Where(p => p.CategoryId == categoryId)
                      .OrderByDescending(p => p.ProductId); // və ya başqa sıralama

            var totalCount = await query.CountAsync();

            var items = await query.Skip((page - 1) * pageSize)
                                   .Take(pageSize)
                                   .ToListAsync();

            return new PaginatedResult<Product>
            {
                Items = items,
                TotalCount = totalCount,
                PageSize = pageSize,
                CurrentPage = page
            };
        }
      

        public async Task<List<Product>> GetProductByCategory(int? categoryId)
        {
            return await _context.Products.Where(p => p.CategoryId == categoryId).ToListAsync();
        }

        public async Task<Product> GetProductByIdWithNotes(int id)
        {
            return await _context.Products
                .Include(p=>p.Brand)
                .Include(p=>p.Family)
                .Include(p=>p.Category)
                .Include(p => p.ProductNotes)
                .ThenInclude(pn=>pn.FragranceNote)
                .ThenInclude(fn=>fn.NoteTypes)
                .FirstOrDefaultAsync(p => p.ProductId == id);
        }

        public Task<List<Product>> GetProductByPriceFilter(int? Minprice, int? Maxprice)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Product>> GetProductBySearch(string search)
        {
            return await _context.Products.Where(p => p.Name.Contains(search)).ToListAsync();
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
