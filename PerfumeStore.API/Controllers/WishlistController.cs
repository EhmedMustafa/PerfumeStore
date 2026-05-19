using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PerfumeStore.Domain.Entities;
using PerfumeStore.Infrastructure.Data;

namespace PerfumeStore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class WishlistController : ControllerBase
    {
        private readonly AppDbContext _db;

        public WishlistController(AppDbContext db) { _db = db; }

        public class AddDto { public int ProductId { get; set; } }
        public class MergeDto { public int[] ProductIds { get; set; } }

        private int GetUserId()
        {
            var claim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                        ?? User.FindFirst("nameid")?.Value
                        ?? User.FindFirst("sub")?.Value;
            return int.TryParse(claim, out var id) ? id : 0;
        }

        // GET /api/Wishlist — istifadəçinin wishlist-i (productId list)
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var userId = GetUserId();
            if (userId == 0) return Unauthorized();
            var ids = await _db.WishlistItems
                .Where(w => w.AppUserId == userId)
                .OrderByDescending(w => w.CreatedAt)
                .Select(w => w.ProductId)
                .ToListAsync();
            return Ok(ids);
        }

        // POST /api/Wishlist — bir məhsul əlavə et
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] AddDto dto)
        {
            var userId = GetUserId();
            if (userId == 0) return Unauthorized();
            if (dto == null || dto.ProductId <= 0) return BadRequest(new { message = "ProductId məcburidir" });

            var exists = await _db.WishlistItems.AnyAsync(w => w.AppUserId == userId && w.ProductId == dto.ProductId);
            if (!exists)
            {
                var productExists = await _db.Products.AnyAsync(p => p.ProductId == dto.ProductId);
                if (!productExists) return NotFound(new { message = "Məhsul tapılmadı" });

                _db.WishlistItems.Add(new WishlistItem { AppUserId = userId, ProductId = dto.ProductId });
                await _db.SaveChangesAsync();
            }
            return Ok(new { message = "Əlavə olundu" });
        }

        // DELETE /api/Wishlist/{productId}
        [HttpDelete("{productId}")]
        public async Task<IActionResult> Remove(int productId)
        {
            var userId = GetUserId();
            if (userId == 0) return Unauthorized();

            var item = await _db.WishlistItems.FirstOrDefaultAsync(w => w.AppUserId == userId && w.ProductId == productId);
            if (item != null)
            {
                _db.WishlistItems.Remove(item);
                await _db.SaveChangesAsync();
            }
            return Ok(new { message = "Silindi" });
        }

        // POST /api/Wishlist/merge — login zamanı localStorage wishlist-i serverə qoş
        [HttpPost("merge")]
        public async Task<IActionResult> Merge([FromBody] MergeDto dto)
        {
            var userId = GetUserId();
            if (userId == 0) return Unauthorized();
            if (dto?.ProductIds == null || dto.ProductIds.Length == 0) return await Get();

            var ids = dto.ProductIds.Distinct().Where(id => id > 0).ToList();
            if (ids.Count == 0) return await Get();

            // Mövcud ID-ləri tap (məhsul DB-də olmalıdır)
            var existingProductIds = await _db.Products
                .Where(p => ids.Contains(p.ProductId))
                .Select(p => p.ProductId)
                .ToListAsync();

            var alreadyInWishlist = await _db.WishlistItems
                .Where(w => w.AppUserId == userId && existingProductIds.Contains(w.ProductId))
                .Select(w => w.ProductId)
                .ToListAsync();

            var toAdd = existingProductIds.Except(alreadyInWishlist)
                .Select(pid => new WishlistItem { AppUserId = userId, ProductId = pid });

            _db.WishlistItems.AddRange(toAdd);
            await _db.SaveChangesAsync();

            return await Get();
        }
    }
}
