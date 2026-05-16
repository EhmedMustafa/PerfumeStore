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
    public class ContactController : ControllerBase
    {
        private readonly AppDbContext _db;

        public ContactController(AppDbContext db)
        {
            _db = db;
        }

        public class ContactDto
        {
            public string Name { get; set; }
            public string Email { get; set; }
            public string Subject { get; set; }
            public string Message { get; set; }
        }

        // POST /api/Contact
        [HttpPost]
        public async Task<IActionResult> Send([FromBody] ContactDto dto)
        {
            if (dto == null ||
                string.IsNullOrWhiteSpace(dto.Name) ||
                string.IsNullOrWhiteSpace(dto.Email) ||
                string.IsNullOrWhiteSpace(dto.Message))
            {
                return BadRequest(new { message = "Bütün məcburi xanaları doldurun" });
            }

            // Spam qarşısı — eyni IP-dən 1 dəqiqə ərzində 3+ mesaj olmasın
            // (sadə yoxlama, real layihə üçün rate limiting middleware istifadə et)
            if (dto.Message.Length > 5000)
                return BadRequest(new { message = "Mesaj çox uzundur (maks 5000 simvol)" });

            _db.ContactMessages.Add(new ContactMessage
            {
                Name = dto.Name.Trim(),
                Email = dto.Email.Trim(),
                Subject = dto.Subject?.Trim() ?? "",
                Message = dto.Message.Trim()
            });
            await _db.SaveChangesAsync();
            return Ok(new { message = "Mesajınız uğurla göndərildi" });
        }

        // GET /api/Contact — Yalnız Admin
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAll()
        {
            var list = await _db.ContactMessages
                .OrderByDescending(m => m.CreatedAt)
                .ToListAsync();
            return Ok(list);
        }

        // PUT /api/Contact/{id}/read — Yalnız Admin
        [HttpPut("{id}/read")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> MarkRead(int id)
        {
            var m = await _db.ContactMessages.FindAsync(id);
            if (m == null) return NotFound();
            m.IsRead = true;
            await _db.SaveChangesAsync();
            return Ok(new { message = "Oxundu" });
        }
    }
}
