using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace PerfumeStore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class UploadController : ControllerBase
    {
        private readonly IWebHostEnvironment _env;

        private static readonly string[] AllowedExtensions =
            { ".jpg", ".jpeg", ".png", ".webp", ".gif" };
        private const long MaxBytes = 5 * 1024 * 1024; // 5 MB

        public UploadController(IWebHostEnvironment env)
        {
            _env = env;
        }

        // POST /api/Upload/image — multipart/form-data, field: file
        [HttpPost("image")]
        [RequestSizeLimit(10_000_000)] // 10 MB hard cap
        public async Task<IActionResult> UploadImage(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest(new { message = "Fayl seçilməyib" });

            if (file.Length > MaxBytes)
                return BadRequest(new { message = "Fayl çox böyükdür (maks 5 MB)" });

            var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (string.IsNullOrEmpty(ext) || !AllowedExtensions.Contains(ext))
                return BadRequest(new { message = "Yalnız jpg, jpeg, png, webp, gif" });

            // wwwroot yoxdursa yarat
            var webRoot = _env.WebRootPath;
            if (string.IsNullOrEmpty(webRoot))
            {
                webRoot = Path.Combine(_env.ContentRootPath, "wwwroot");
            }
            var uploadDir = Path.Combine(webRoot, "uploads", "products");
            Directory.CreateDirectory(uploadDir);

            // Unique fayl adı
            var fileName = $"{Guid.NewGuid():N}{ext}";
            var fullPath = Path.Combine(uploadDir, fileName);

            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // Frontend-in istifadə edəcəyi tam URL
            var publicPath = $"/uploads/products/{fileName}";
            var absoluteUrl = $"{Request.Scheme}://{Request.Host}{publicPath}";

            return Ok(new { url = absoluteUrl, path = publicPath, fileName, size = file.Length });
        }
    }
}
