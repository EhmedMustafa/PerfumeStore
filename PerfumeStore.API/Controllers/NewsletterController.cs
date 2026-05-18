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
    public class NewsletterController : ControllerBase
    {
        private readonly AppDbContext _db;

        public NewsletterController(AppDbContext db)
        {
            _db = db;
        }

        public class SubscribeDto
        {
            public string Email { get; set; }
        }

        // POST /api/Newsletter/subscribe
        [HttpPost("subscribe")]
        public async Task<IActionResult> Subscribe([FromBody] SubscribeDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto?.Email))
                return BadRequest(new { message = "E-poçt boş ola bilməz" });

            var email = dto.Email.Trim().ToLowerInvariant();
            if (!email.Contains('@') || !email.Contains('.'))
                return BadRequest(new { message = "Düzgün e-poçt daxil edin" });

            var existing = await _db.NewsletterSubscribers
                .FirstOrDefaultAsync(s => s.Email == email);

            if (existing != null)
            {
                if (!existing.IsActive)
                {
                    existing.IsActive = true;
                    await _db.SaveChangesAsync();
                }
                return Ok(new { message = "Artıq abunəsiniz" });
            }

            _db.NewsletterSubscribers.Add(new NewsletterSubscriber { Email = email });
            await _db.SaveChangesAsync();
            return Ok(new { message = "Abunəliyiniz təsdiqləndi" });
        }

        // GET /api/Newsletter — Yalnız Admin
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAll()
        {
            var list = await _db.NewsletterSubscribers
                .OrderByDescending(s => s.SubscribedAt)
                .ToListAsync();
            return Ok(list);
        }

        // DELETE /api/Newsletter/{id} — Yalnız Admin
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var sub = await _db.NewsletterSubscribers.FindAsync(id);
            if (sub == null) return NotFound();
            _db.NewsletterSubscribers.Remove(sub);
            await _db.SaveChangesAsync();
            return Ok(new { message = "Silindi" });
        }
    }
}
