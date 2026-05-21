using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
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
    public class BlogController : ControllerBase
    {
        private readonly AppDbContext _db;
        public BlogController(AppDbContext db) { _db = db; }

        public class BlogArticleDto
        {
            public int Id { get; set; }
            public string Slug { get; set; }
            public string Title { get; set; }
            public string Excerpt { get; set; }
            public string Tag { get; set; }
            public string ImageUrl { get; set; }
            public List<string> Body { get; set; } = new();
            public DateTime PublishedAt { get; set; }
            public int ViewCount { get; set; }
            public bool IsPublished { get; set; }
        }

        public class BlogArticleCreateDto
        {
            public string Slug { get; set; }
            public string Title { get; set; }
            public string Excerpt { get; set; }
            public string Tag { get; set; }
            public string ImageUrl { get; set; }
            public List<string> Body { get; set; } = new();
            public bool IsPublished { get; set; } = true;
        }

        public class BlogArticleUpdateDto : BlogArticleCreateDto
        {
            public int Id { get; set; }
        }

        private static BlogArticleDto ToDto(BlogArticle a)
        {
            List<string> body;
            try { body = JsonSerializer.Deserialize<List<string>>(a.BodyJson ?? "[]") ?? new(); }
            catch { body = new(); }
            return new BlogArticleDto
            {
                Id = a.Id,
                Slug = a.Slug,
                Title = a.Title,
                Excerpt = a.Excerpt,
                Tag = a.Tag,
                ImageUrl = a.ImageUrl,
                Body = body,
                PublishedAt = a.PublishedAt,
                ViewCount = a.ViewCount,
                IsPublished = a.IsPublished
            };
        }

        private static string SlugifyAz(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return Guid.NewGuid().ToString("N").Substring(0, 8);
            var s = input.Trim().ToLowerInvariant()
                .Replace("ə", "e").Replace("ı", "i").Replace("ö", "o").Replace("ü", "u")
                .Replace("ç", "c").Replace("ş", "s").Replace("ğ", "g");
            var sb = new System.Text.StringBuilder();
            foreach (var ch in s)
            {
                if (char.IsLetterOrDigit(ch)) sb.Append(ch);
                else if (ch == ' ' || ch == '-' || ch == '_') sb.Append('-');
            }
            var result = System.Text.RegularExpressions.Regex.Replace(sb.ToString(), "-+", "-").Trim('-');
            return string.IsNullOrEmpty(result) ? Guid.NewGuid().ToString("N").Substring(0, 8) : result;
        }

        // GET /api/Blog — public yalnız published; ?all=true admin üçün
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] bool all = false)
        {
            var q = _db.BlogArticles.AsQueryable();
            if (!all) q = q.Where(a => a.IsPublished);
            var list = await q.OrderByDescending(a => a.PublishedAt).ToListAsync();
            return Ok(list.Select(ToDto));
        }

        [HttpGet("{slug}")]
        public async Task<IActionResult> GetBySlug(string slug)
        {
            var a = await _db.BlogArticles.FirstOrDefaultAsync(x => x.Slug == slug);
            if (a == null) return NotFound();
            return Ok(ToDto(a));
        }

        // POST /api/Blog/{id}/view — anonymous, view sayğacını artırır
        [HttpPost("{id}/view")]
        public async Task<IActionResult> IncrementView(int id)
        {
            var a = await _db.BlogArticles.FindAsync(id);
            if (a == null) return NotFound();
            a.ViewCount += 1;
            await _db.SaveChangesAsync();
            return Ok(new { viewCount = a.ViewCount });
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] BlogArticleCreateDto dto)
        {
            if (dto == null) return BadRequest(new { message = "Boş sorğu" });
            if (string.IsNullOrWhiteSpace(dto.Title)) return BadRequest(new { message = "Başlıq məcburidir" });

            var slug = string.IsNullOrWhiteSpace(dto.Slug) ? SlugifyAz(dto.Title) : SlugifyAz(dto.Slug);
            // unique check
            var baseSlug = slug; var i = 1;
            while (await _db.BlogArticles.AnyAsync(x => x.Slug == slug)) { slug = $"{baseSlug}-{++i}"; }

            var entity = new BlogArticle
            {
                Slug = slug,
                Title = dto.Title.Trim(),
                Excerpt = dto.Excerpt ?? "",
                Tag = dto.Tag ?? "",
                ImageUrl = dto.ImageUrl ?? "",
                BodyJson = JsonSerializer.Serialize(dto.Body ?? new List<string>()),
                IsPublished = dto.IsPublished,
                PublishedAt = DateTime.UtcNow,
                ViewCount = 0
            };
            _db.BlogArticles.Add(entity);
            await _db.SaveChangesAsync();
            return Ok(new { id = entity.Id, slug = entity.Slug, message = "Məqalə əlavə olundu" });
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update([FromBody] BlogArticleUpdateDto dto)
        {
            if (dto == null || dto.Id <= 0) return BadRequest(new { message = "ID məcburidir" });
            var entity = await _db.BlogArticles.FirstOrDefaultAsync(x => x.Id == dto.Id);
            if (entity == null) return NotFound();

            entity.Title = (dto.Title ?? "").Trim();
            entity.Excerpt = dto.Excerpt ?? "";
            entity.Tag = dto.Tag ?? "";
            entity.ImageUrl = dto.ImageUrl ?? "";
            entity.BodyJson = JsonSerializer.Serialize(dto.Body ?? new List<string>());
            entity.IsPublished = dto.IsPublished;

            // slug change (qoruyaq unique)
            if (!string.IsNullOrWhiteSpace(dto.Slug))
            {
                var newSlug = SlugifyAz(dto.Slug);
                if (newSlug != entity.Slug)
                {
                    var baseSlug = newSlug; var i = 1;
                    while (await _db.BlogArticles.AnyAsync(x => x.Slug == newSlug && x.Id != entity.Id))
                    {
                        newSlug = $"{baseSlug}-{++i}";
                    }
                    entity.Slug = newSlug;
                }
            }

            await _db.SaveChangesAsync();
            return Ok(new { message = "Yeniləndi", slug = entity.Slug });
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var a = await _db.BlogArticles.FindAsync(id);
            if (a == null) return NotFound();
            _db.BlogArticles.Remove(a);
            await _db.SaveChangesAsync();
            return Ok(new { message = "Silindi" });
        }
    }
}
