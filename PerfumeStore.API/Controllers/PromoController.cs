using System;
using System.Linq;
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
    public class PromoController : ControllerBase
    {
        private readonly AppDbContext _db;
        public PromoController(AppDbContext db) { _db = db; }

        public class PromoCreateDto
        {
            public string Code { get; set; }
            public int DiscountPercent { get; set; }
            public DateTime? ValidFrom { get; set; }
            public DateTime? ValidUntil { get; set; }
            public int? UsageLimit { get; set; }
            public decimal? MinOrderAmount { get; set; }
            public bool FirstOrderOnly { get; set; }
            public bool IsActive { get; set; } = true;
            public string Description { get; set; }
        }

        public class PromoUpdateDto : PromoCreateDto
        {
            public int Id { get; set; }
        }

        public class PromoValidateDto
        {
            public string Code { get; set; }
            public decimal Subtotal { get; set; }
        }

        // Admin — bütün promo-lar
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAll()
        {
            var list = await _db.Promos.OrderByDescending(p => p.CreatedAt).ToListAsync();
            return Ok(list);
        }

        // Public — ana səhifə banner-i üçün aktiv promo
        // Prioritet: countdown olanlar (validUntil-i ən yaxın bitənlər) → countdown olmayanlar
        [HttpGet("active-countdown")]
        public async Task<IActionResult> GetActiveCountdown()
        {
            var now = DateTime.UtcNow;
            var allActive = await _db.Promos
                .Where(p => p.IsActive
                    && (!p.ValidFrom.HasValue || p.ValidFrom <= now)
                    && (!p.ValidUntil.HasValue || p.ValidUntil > now)
                    && (!p.UsageLimit.HasValue || p.UsedCount < p.UsageLimit))
                .ToListAsync();

            // Əvvəl validUntil olanı tap (countdown göstərmək üçün), yoxdursa istənilən birini
            var promo = allActive
                .OrderBy(p => p.ValidUntil.HasValue ? 0 : 1)
                .ThenBy(p => p.ValidUntil)
                .FirstOrDefault();

            if (promo == null) return Ok(null);
            return Ok(new
            {
                code = promo.Code,
                discountPercent = promo.DiscountPercent,
                validUntil = promo.ValidUntil,
                description = promo.Description,
                firstOrderOnly = promo.FirstOrderOnly,
                minOrderAmount = promo.MinOrderAmount
            });
        }

        // Public — kodu yoxla (checkout-da)
        [HttpPost("validate")]
        public async Task<IActionResult> Validate([FromBody] PromoValidateDto dto)
        {
            if (dto == null || string.IsNullOrWhiteSpace(dto.Code))
                return BadRequest(new { valid = false, message = "Kod boşdur" });

            var code = dto.Code.Trim().ToUpperInvariant();
            var promo = await _db.Promos.FirstOrDefaultAsync(p => p.Code.ToUpper() == code);
            if (promo == null) return Ok(new { valid = false, message = "Kod tapılmadı" });
            if (!promo.IsActive) return Ok(new { valid = false, message = "Bu kod aktiv deyil" });

            var now = DateTime.UtcNow;
            if (promo.ValidFrom.HasValue && promo.ValidFrom > now)
                return Ok(new { valid = false, message = $"Bu kod {promo.ValidFrom:dd.MM.yyyy}-dən aktivdir" });
            if (promo.ValidUntil.HasValue && promo.ValidUntil < now)
                return Ok(new { valid = false, message = "Bu kodun müddəti bitib" });
            if (promo.UsageLimit.HasValue && promo.UsedCount >= promo.UsageLimit)
                return Ok(new { valid = false, message = "Bu kodun istifadə limiti dolub" });
            if (promo.MinOrderAmount.HasValue && dto.Subtotal < promo.MinOrderAmount)
                return Ok(new { valid = false, message = $"Minimum sifariş məbləği: ₼{promo.MinOrderAmount:0.00}" });

            // İlk sifariş yoxlaması (yalnız auth-da)
            if (promo.FirstOrderOnly)
            {
                var userIdClaim = User?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userIdClaim))
                    return Ok(new { valid = false, message = "Bu kod yalnız qeydiyyatdan keçmiş istifadəçilər üçündür" });
                if (int.TryParse(userIdClaim, out var uid))
                {
                    var hasOrders = await _db.Orders.AnyAsync(o => o.UserId == uid);
                    if (hasOrders)
                        return Ok(new { valid = false, message = "Bu kod yalnız ilk sifariş üçündür" });
                }
            }

            return Ok(new
            {
                valid = true,
                code = promo.Code,
                discountPercent = promo.DiscountPercent,
                discountAmount = Math.Round(dto.Subtotal * promo.DiscountPercent / 100m, 2),
                message = $"Kod tətbiq olundu — {promo.DiscountPercent}% endirim"
            });
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] PromoCreateDto dto)
        {
            if (dto == null) return BadRequest(new { message = "Boş sorğu" });
            if (string.IsNullOrWhiteSpace(dto.Code)) return BadRequest(new { message = "Kod məcburidir" });
            if (dto.DiscountPercent <= 0 || dto.DiscountPercent > 100)
                return BadRequest(new { message = "Endirim 1-100 arası olmalıdır" });

            var code = dto.Code.Trim().ToUpperInvariant();
            if (await _db.Promos.AnyAsync(p => p.Code.ToUpper() == code))
                return BadRequest(new { message = "Bu kod artıq mövcuddur" });

            var entity = new Promo
            {
                Code = code,
                DiscountPercent = dto.DiscountPercent,
                ValidFrom = dto.ValidFrom,
                ValidUntil = dto.ValidUntil,
                UsageLimit = dto.UsageLimit,
                MinOrderAmount = dto.MinOrderAmount,
                FirstOrderOnly = dto.FirstOrderOnly,
                IsActive = dto.IsActive,
                Description = dto.Description ?? ""
            };
            _db.Promos.Add(entity);
            await _db.SaveChangesAsync();
            return Ok(new { id = entity.Id, message = "Promo yaradıldı" });
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update([FromBody] PromoUpdateDto dto)
        {
            if (dto == null || dto.Id <= 0) return BadRequest(new { message = "ID məcburidir" });
            var entity = await _db.Promos.FindAsync(dto.Id);
            if (entity == null) return NotFound();

            var newCode = (dto.Code ?? "").Trim().ToUpperInvariant();
            if (string.IsNullOrEmpty(newCode))
                return BadRequest(new { message = "Kod məcburidir" });
            if (newCode != entity.Code && await _db.Promos.AnyAsync(p => p.Code.ToUpper() == newCode && p.Id != entity.Id))
                return BadRequest(new { message = "Bu kod artıq mövcuddur" });

            entity.Code = newCode;
            entity.DiscountPercent = dto.DiscountPercent;
            entity.ValidFrom = dto.ValidFrom;
            entity.ValidUntil = dto.ValidUntil;
            entity.UsageLimit = dto.UsageLimit;
            entity.MinOrderAmount = dto.MinOrderAmount;
            entity.FirstOrderOnly = dto.FirstOrderOnly;
            entity.IsActive = dto.IsActive;
            entity.Description = dto.Description ?? "";

            await _db.SaveChangesAsync();
            return Ok(new { message = "Yeniləndi" });
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var p = await _db.Promos.FindAsync(id);
            if (p == null) return NotFound();
            _db.Promos.Remove(p);
            await _db.SaveChangesAsync();
            return Ok(new { message = "Silindi" });
        }
    }
}
