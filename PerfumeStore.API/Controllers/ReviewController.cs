using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PerfumeStore.Domain.Entities;
using PerfumeStore.Domain.Entities.Identity;
using PerfumeStore.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;

namespace PerfumeStore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly UserManager<AppUser> _userManager;

        public ReviewController(AppDbContext db, UserManager<AppUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        public class CreateReviewDto
        {
            public int ProductId { get; set; }
            public int Rating { get; set; }
            public string Comment { get; set; }
        }

        public class ResultReviewDto
        {
            public int Id { get; set; }
            public int ProductId { get; set; }
            public int Rating { get; set; }
            public string Comment { get; set; }
            public string UserDisplayName { get; set; }
            public DateTime CreatedAt { get; set; }
        }

        public class ProductRatingSummaryDto
        {
            public int ProductId { get; set; }
            public int Count { get; set; }
            public double Average { get; set; }
        }

        private int GetCurrentUserId()
        {
            var claim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                        ?? User.FindFirst("nameid")?.Value
                        ?? User.FindFirst("sub")?.Value;
            return int.TryParse(claim, out var id) ? id : 0;
        }

        // GET /api/Review/product/5
        [HttpGet("product/{productId}")]
        public async Task<IActionResult> GetByProduct(int productId)
        {
            var list = await _db.Reviews
                .Where(r => r.ProductId == productId && r.IsApproved)
                .OrderByDescending(r => r.CreatedAt)
                .Select(r => new ResultReviewDto
                {
                    Id = r.Id,
                    ProductId = r.ProductId,
                    Rating = r.Rating,
                    Comment = r.Comment,
                    UserDisplayName = r.UserDisplayName,
                    CreatedAt = r.CreatedAt
                })
                .ToListAsync();
            return Ok(list);
        }

        // GET /api/Review/summary/5
        [HttpGet("summary/{productId}")]
        public async Task<IActionResult> GetSummary(int productId)
        {
            var query = _db.Reviews.Where(r => r.ProductId == productId && r.IsApproved);
            var count = await query.CountAsync();
            var avg = count > 0 ? await query.AverageAsync(r => (double)r.Rating) : 0.0;
            return Ok(new ProductRatingSummaryDto
            {
                ProductId = productId,
                Count = count,
                Average = Math.Round(avg, 2)
            });
        }

        // GET /api/Review/summary?ids=1,2,3
        [HttpGet("summary")]
        public async Task<IActionResult> GetBulkSummary([FromQuery] string ids)
        {
            if (string.IsNullOrWhiteSpace(ids))
                return Ok(new ProductRatingSummaryDto[0]);

            var idList = ids.Split(',')
                .Select(s => int.TryParse(s.Trim(), out var v) ? v : 0)
                .Where(v => v > 0)
                .ToList();

            if (!idList.Any()) return Ok(new ProductRatingSummaryDto[0]);

            var data = await _db.Reviews
                .Where(r => idList.Contains(r.ProductId) && r.IsApproved)
                .GroupBy(r => r.ProductId)
                .Select(g => new ProductRatingSummaryDto
                {
                    ProductId = g.Key,
                    Count = g.Count(),
                    Average = Math.Round(g.Average(x => (double)x.Rating), 2)
                })
                .ToListAsync();

            return Ok(data);
        }

        // POST /api/Review
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create([FromBody] CreateReviewDto dto)
        {
            if (dto == null) return BadRequest(new { message = "Boş sorğu" });
            if (dto.Rating < 1 || dto.Rating > 5)
                return BadRequest(new { message = "Reytinq 1–5 arası olmalıdır" });
            if (string.IsNullOrWhiteSpace(dto.Comment) || dto.Comment.Trim().Length < 5)
                return BadRequest(new { message = "Şərh ən azı 5 simvol olmalıdır" });
            if (dto.Comment.Length > 1000)
                return BadRequest(new { message = "Şərh çox uzundur (maks 1000)" });

            var userId = GetCurrentUserId();
            if (userId == 0) return Unauthorized(new { message = "İstifadəçi tanınmadı" });

            var productExists = await _db.Products.AnyAsync(p => p.ProductId == dto.ProductId);
            if (!productExists) return NotFound(new { message = "Məhsul tapılmadı" });

            var existing = await _db.Reviews
                .FirstOrDefaultAsync(r => r.ProductId == dto.ProductId && r.AppUserId == userId);

            var user = await _userManager.FindByIdAsync(userId.ToString());
            var displayName = user != null
                ? (!string.IsNullOrWhiteSpace(user.FirstName)
                    ? $"{user.FirstName} {(user.LastName ?? "").FirstOrDefault()}".Trim() + "."
                    : (user.UserName ?? "İstifadəçi"))
                : "İstifadəçi";

            if (existing != null)
            {
                existing.Rating = dto.Rating;
                existing.Comment = dto.Comment.Trim();
                existing.CreatedAt = DateTime.UtcNow;
                existing.UserDisplayName = displayName;
            }
            else
            {
                _db.Reviews.Add(new Review
                {
                    ProductId = dto.ProductId,
                    AppUserId = userId,
                    Rating = dto.Rating,
                    Comment = dto.Comment.Trim(),
                    UserDisplayName = displayName,
                    CreatedAt = DateTime.UtcNow,
                    IsApproved = true
                });
            }

            await _db.SaveChangesAsync();
            return Ok(new { message = "Şərh yadda saxlandı" });
        }

        // DELETE /api/Review/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            var review = await _db.Reviews.FindAsync(id);
            if (review == null) return NotFound();

            var userId = GetCurrentUserId();
            var isAdmin = User.IsInRole("Admin");
            if (!isAdmin && review.AppUserId != userId)
                return Forbid();

            _db.Reviews.Remove(review);
            await _db.SaveChangesAsync();
            return Ok(new { message = "Şərh silindi" });
        }

        // GET /api/Review — Admin: bütün şərhlər (məhsul adı ilə)
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAll()
        {
            var list = await _db.Reviews
                .OrderByDescending(r => r.CreatedAt)
                .Join(_db.Products,
                      r => r.ProductId,
                      p => p.ProductId,
                      (r, p) => new
                      {
                          r.Id,
                          r.ProductId,
                          ProductName = p.Name,
                          r.Rating,
                          r.Comment,
                          r.UserDisplayName,
                          r.CreatedAt,
                          r.IsApproved
                      })
                .ToListAsync();
            return Ok(list);
        }

        // PUT /api/Review/{id}/approve — Admin
        public class ApproveDto { public bool IsApproved { get; set; } }

        [HttpPut("{id}/approve")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> SetApproval(int id, [FromBody] ApproveDto dto)
        {
            var r = await _db.Reviews.FindAsync(id);
            if (r == null) return NotFound();
            r.IsApproved = dto?.IsApproved ?? !r.IsApproved;
            await _db.SaveChangesAsync();
            return Ok(new { message = r.IsApproved ? "Təsdiqləndi" : "Gizlədildi", isApproved = r.IsApproved });
        }
    }
}
