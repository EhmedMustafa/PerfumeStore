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
    public class BannerController : ControllerBase
    {
        private readonly AppDbContext _db;
        public BannerController(AppDbContext db) { _db = db; }

        public class BannerCreateDto
        {
            public string Title { get; set; }
            public string Subtitle { get; set; }
            public string ImageUrl { get; set; }
            public string CtaText { get; set; }
            public string CtaLink { get; set; }
            public string Theme { get; set; } = "dark";
            public bool IsActive { get; set; } = true;
            public int SortOrder { get; set; } = 0;
        }

        public class BannerUpdateDto : BannerCreateDto
        {
            public int Id { get; set; }
        }

        // GET /api/Banner — public: yalnız aktiv; ?all=true admin üçün
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] bool all = false)
        {
            var q = _db.HomeBanners.AsQueryable();
            if (!all) q = q.Where(b => b.IsActive);
            var list = await q.OrderBy(b => b.SortOrder).ThenByDescending(b => b.CreatedAt).ToListAsync();
            return Ok(list);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var b = await _db.HomeBanners.FindAsync(id);
            if (b == null) return NotFound();
            return Ok(b);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] BannerCreateDto dto)
        {
            if (dto == null) return BadRequest(new { message = "Boş sorğu" });
            var entity = new HomeBanner
            {
                Title = dto.Title ?? "",
                Subtitle = dto.Subtitle ?? "",
                ImageUrl = dto.ImageUrl ?? "",
                CtaText = dto.CtaText ?? "",
                CtaLink = dto.CtaLink ?? "",
                Theme = string.IsNullOrWhiteSpace(dto.Theme) ? "dark" : dto.Theme.Trim().ToLowerInvariant(),
                IsActive = dto.IsActive,
                SortOrder = dto.SortOrder
            };
            _db.HomeBanners.Add(entity);
            await _db.SaveChangesAsync();
            return Ok(new { id = entity.Id, message = "Banner əlavə olundu" });
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update([FromBody] BannerUpdateDto dto)
        {
            if (dto == null || dto.Id <= 0) return BadRequest(new { message = "ID məcburidir" });
            var entity = await _db.HomeBanners.FindAsync(dto.Id);
            if (entity == null) return NotFound();

            entity.Title = dto.Title ?? "";
            entity.Subtitle = dto.Subtitle ?? "";
            entity.ImageUrl = dto.ImageUrl ?? "";
            entity.CtaText = dto.CtaText ?? "";
            entity.CtaLink = dto.CtaLink ?? "";
            entity.Theme = string.IsNullOrWhiteSpace(dto.Theme) ? "dark" : dto.Theme.Trim().ToLowerInvariant();
            entity.IsActive = dto.IsActive;
            entity.SortOrder = dto.SortOrder;

            await _db.SaveChangesAsync();
            return Ok(new { message = "Yeniləndi" });
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var b = await _db.HomeBanners.FindAsync(id);
            if (b == null) return NotFound();
            _db.HomeBanners.Remove(b);
            await _db.SaveChangesAsync();
            return Ok(new { message = "Silindi" });
        }
    }
}
