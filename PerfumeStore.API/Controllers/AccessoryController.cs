using System.Collections.Generic;
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
    public class AccessoryController : ControllerBase
    {
        private readonly AppDbContext _db;
        public AccessoryController(AppDbContext db) { _db = db; }

        public class VariantDto
        {
            public int Id { get; set; }
            public string Size { get; set; }
            public decimal Price { get; set; }
        }

        public class AccessoryCreateDto
        {
            public string Name { get; set; }
            public string Description { get; set; }
            public string ImageUrl { get; set; }
            public string Type { get; set; }
            public bool IsAvailable { get; set; } = true;
            public List<VariantDto> Variants { get; set; } = new();
        }

        public class AccessoryUpdateDto : AccessoryCreateDto
        {
            public int Id { get; set; }
        }

        // GET /api/Accessory — public list (yalnız mövcud olanlar)
        // ?all=true → admin üçün hamısı (auth yox, sadəlik üçün)
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] bool all = false)
        {
            var q = _db.Accessories.Include(a => a.Variants).AsQueryable();
            if (!all) q = q.Where(a => a.IsAvailable);
            var list = await q.OrderByDescending(a => a.CreatedAt).ToListAsync();
            return Ok(list);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var a = await _db.Accessories.Include(x => x.Variants).FirstOrDefaultAsync(x => x.Id == id);
            if (a == null) return NotFound();
            return Ok(a);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] AccessoryCreateDto dto)
        {
            if (dto == null) return BadRequest(new { message = "Boş sorğu" });
            if (string.IsNullOrWhiteSpace(dto.Name)) return BadRequest(new { message = "Ad məcburidir" });

            var entity = new Accessory
            {
                Name = dto.Name.Trim(),
                Description = dto.Description ?? "",
                ImageUrl = dto.ImageUrl ?? "",
                Type = string.IsNullOrWhiteSpace(dto.Type) ? "flakon" : dto.Type.Trim().ToLowerInvariant(),
                IsAvailable = dto.IsAvailable,
                Variants = (dto.Variants ?? new()).Select(v => new AccessoryVariant
                {
                    Size = v.Size ?? "",
                    Price = v.Price
                }).ToList()
            };
            _db.Accessories.Add(entity);
            await _db.SaveChangesAsync();
            return Ok(new { id = entity.Id, message = "Aksessuar əlavə olundu" });
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update([FromBody] AccessoryUpdateDto dto)
        {
            if (dto == null || dto.Id <= 0) return BadRequest(new { message = "ID məcburidir" });

            var entity = await _db.Accessories.Include(a => a.Variants).FirstOrDefaultAsync(a => a.Id == dto.Id);
            if (entity == null) return NotFound();

            entity.Name = (dto.Name ?? "").Trim();
            entity.Description = dto.Description ?? "";
            entity.ImageUrl = dto.ImageUrl ?? "";
            entity.Type = string.IsNullOrWhiteSpace(dto.Type) ? "flakon" : dto.Type.Trim().ToLowerInvariant();
            entity.IsAvailable = dto.IsAvailable;

            // Köhnə variantları sil, təzələri əlavə et
            _db.AccessoryVariants.RemoveRange(entity.Variants);
            entity.Variants = (dto.Variants ?? new()).Select(v => new AccessoryVariant
            {
                Size = v.Size ?? "",
                Price = v.Price
            }).ToList();

            await _db.SaveChangesAsync();
            return Ok(new { message = "Aksessuar yeniləndi" });
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var a = await _db.Accessories.FindAsync(id);
            if (a == null) return NotFound();
            _db.Accessories.Remove(a);
            await _db.SaveChangesAsync();
            return Ok(new { message = "Silindi" });
        }
    }
}
