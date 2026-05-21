using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using System.Threading.RateLimiting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PerfumeStore.API.Middleware;
using PerfumeStore.Application;
using PerfumeStore.Application.Services;
using PerfumeStore.Domain.Entities.Identity;
using PerfumeStore.Infrastructure;
using PerfumeStore.Infrastructure.Data;
using PerfumeStore.Infrastructure.Services;
using PerfumeStore.Application.Services.CategoryServices;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

var con = builder.Configuration;
builder.Services.AddApplication();
builder.Services.AddInfrastructure(con);
builder.Services
    .AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        // Object cycle-ı görsə crash etmə — yenidən qarşılaşan referansları atla
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
        // Default ignore null, response sadələşsin
        options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
    });
// Add services to the container.


// ✅ Identity Konfiqurasiyası
builder.Services.AddIdentity<AppUser, AppRole>(options =>
{
    options.Password.RequireDigit = false;//Şifrədə ən azı 1 rəqəm olmalıdır.
    options.Password.RequiredLength = 6;// Minimum 6 simvol uzunluğunda olmalıdır.
    options.Password.RequireNonAlphanumeric = false;//Xüsusi simvollar (@, #, !) tələb olunmur.
    options.Password.RequireUppercase = false;//Böyük hərf məcburi deyil.
    options.Password.RequireLowercase = false;

    options.User.RequireUniqueEmail = true;

    options.SignIn.RequireConfirmedEmail = false;
})
.AddEntityFrameworkStores<AppDbContext>()// Identity verilənlər bazası AppDbContext vasitəsilə saxlanılır.
.AddDefaultTokenProviders();// Şifrə yeniləmə, email təsdiqi üçün tokenlər yaradacaq.


// JWT Authentication
// Mühüm: AddIdentity() default cookie scheme təyin edir.
// JWT-ni default-a çevirmək üçün options-ları açıq qeyd edirik.
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options =>
    {
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = con["Jwt:Issuer"],
            ValidAudience = con["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(con["Jwt:Key"]!)),
            ClockSkew = TimeSpan.FromMinutes(5),
            NameClaimType = System.Security.Claims.ClaimTypes.Name,
            RoleClaimType = System.Security.Claims.ClaimTypes.Role
        };
    });




builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminPolicy", policy => policy.RequireRole("Admin"));
});


// AddControllers artıq yuxarıda qeydiyyat olunub
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ===== Rate Limiting =====
// Login/register flood-a qarşı: IP başına 1 dəqiqədə 10 cəhd.
// API ümumi: IP başına dəqiqədə 100 sorğu.
builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

    options.AddPolicy("auth", httpContext =>
        RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown",
            factory: _ => new FixedWindowRateLimiterOptions
            {
                PermitLimit = 10,
                Window = TimeSpan.FromMinutes(1),
                QueueLimit = 0
            }));

    options.AddPolicy("api", httpContext =>
        RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown",
            factory: _ => new FixedWindowRateLimiterOptions
            {
                PermitLimit = 100,
                Window = TimeSpan.FromMinutes(1),
                QueueLimit = 0
            }));
});


// CORS — Development və Production üçün ayrı qaydalar
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        // appsettings.json-dan icazəli origin-lər
        var allowedOrigins = con.GetSection("Cors:AllowedOrigins").Get<string[]>()
            ?? new[] { "http://localhost:5500", "http://127.0.0.1:5500" };

        policy.WithOrigins(allowedOrigins)
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });

    // Development üçün lokal sınaqda hər yerdən gəlsin (yalnız appsettings.Development-də aktiv)
    options.AddPolicy("AllowAll", policy =>
    {
        policy.SetIsOriginAllowed(_ => true)
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<AppDbContext>();

    // Apply any pending migrations
    context.Database.Migrate();

    // ===== Roles + Admin seed =====
    // appsettings.json -> "AdminSeed": { "Email": "you@mail.com" }
    // O e-poçtla qeydiyyatdan keçmiş user avtomatik Admin olur.
    var roleManager = services.GetRequiredService<RoleManager<AppRole>>();
    var userManager = services.GetRequiredService<UserManager<AppUser>>();

    foreach (var roleName in new[] { "Admin", "User" })
    {
        if (!await roleManager.RoleExistsAsync(roleName))
        {
            await roleManager.CreateAsync(new AppRole { Name = roleName });
        }
    }

    var adminEmail = con["AdminSeed:Email"];
    if (!string.IsNullOrWhiteSpace(adminEmail))
    {
        var admin = await userManager.FindByEmailAsync(adminEmail);
        if (admin != null && !await userManager.IsInRoleAsync(admin, "Admin"))
        {
            await userManager.AddToRoleAsync(admin, "Admin");
        }
    }

    // ===== Blog seed (yalnız boş cədvəldə) =====
    if (!await context.BlogArticles.AnyAsync())
    {
        var seedArticles = new[]
        {
            new PerfumeStore.Domain.Entities.BlogArticle
            {
                Slug = "parfum-notlari-nedir",
                Title = "Parfüm notları nədir? Üst, orta və baza notlarına bələdçi",
                Excerpt = "Hər ətrin öz \"piramida\"sı var — üst notlar, orta notlar və baza. Bu notların necə işlədiyini və bir-birini necə tamamladığını öyrən.",
                Tag = "Bələdçi",
                ImageUrl = "/images/rose.png",
                BodyJson = System.Text.Json.JsonSerializer.Serialize(new[]
                {
                    "Parfüm bir təbəqədə deyil, üç təbəqədə açılır. Hər təbəqə \"not\" adlanır və müxtəlif vaxtlarda hiss olunur.",
                    "## Üst notlar",
                    "Üst notlar ətri sıxan kimi ilk 15 dəqiqədə hiss olunan komponentlərdir. Adətən yüngül, uçucu maddələrdən — sitruslardan (limon, berqamot), aromatik bitkilərdən (lavanda, nanə), ya da yüngül meyvələrdən ibarət olur. Bunlar ətrin \"salamlaşması\"dır.",
                    "## Orta notlar (ürək)",
                    "Üst notlar buxarlananda orta notlar ortaya çıxır. Bunlar ətrin \"şəxsiyyəti\"dir. Çiçəklər (qızılgül, yasəmən, ylang-ylang), ədviyyatlar (darçın, hil), və ya meyvələr (alma, şaftalı) olur. 2–4 saat davam edir.",
                    "## Baza notlar",
                    "Ən uzunömürlü komponentlər — oduncaqlar (sandal, ud), amber, vanil, mişk. Bunlar dəridə 6+ saat qala bilər və ətri \"yaddaqalan\" edir.",
                    "Növbəti dəfə ətir alanda bu üç təbəqəni ayrı-ayrı dəyərləndir — bəzən üst notlar bəyənilməsə də, baza notlar səni heyran edə bilər."
                }),
                PublishedAt = new DateTime(2026, 4, 12, 0, 0, 0, DateTimeKind.Utc),
                IsPublished = true
            },
            new PerfumeStore.Domain.Entities.BlogArticle
            {
                Slug = "mokvsumune-gore-etir-secimi",
                Title = "Mövsümə görə ətir necə seçilir?",
                Excerpt = "Yay üçün ağır oud, qış üçün təzə sitrus — niyə yanlış cütlük? Hər mövsümün öz qoxu profili var.",
                Tag = "Məsləhət",
                ImageUrl = "/images/citrus.png",
                BodyJson = System.Text.Json.JsonSerializer.Serialize(new[]
                {
                    "Mövsüm ətrin necə açılmasına birbaşa təsir edir. İsti hava molekulları daha sürətli buxarlandırır, soyuq hava isə onları \"tutur\".",
                    "## Yay (15–35°C)",
                    "Yay üçün yüngül, sitrus və akva ailələri ideal seçimdir. Berqamot, limon, dəniz akkordları, yaşıl çay — bunlar isti havada təzəlik hissi verir. Ağır oud və amber yayda boğucu görünə bilər.",
                    "## Yaz / payız",
                    "Yumşaq çiçəkli kompozisiyalar, yüngül oduncaqlar (sandal, sidr) və meyvəli notlar — bu mövsüm ən geniş seçim verir.",
                    "## Qış (5°C-dən aşağı)",
                    "Soyuq havada güclü ətirlər daha yaxşı işləyir. Oud, amber, vanil, ədviyyatlar (mixək, darçın), gourmand notlar — bunların hamısı qışda daha \"dolğun\" hiss olunur.",
                    "Qaydanın istisnası odur ki, fərdi zövq həmişə birinci yerdədir. Bəlkə də sənin \"imza\" ətrin bütün il boyu eyni qalır — və bu da gözəldir."
                }),
                PublishedAt = new DateTime(2026, 3, 28, 0, 0, 0, DateTimeKind.Utc),
                IsPublished = true
            },
            new PerfumeStore.Domain.Entities.BlogArticle
            {
                Slug = "orijinal-parfum-nece-bilinir",
                Title = "Orijinal parfümü saxtadan necə ayırmaq olar?",
                Excerpt = "Saxta ətirlərin sayı çoxalıb. Qutu, batch kod, qoxunun davamlılığı — diqqət edilməli detalları sadalayırıq.",
                Tag = "Bələdçi",
                ImageUrl = "/images/vanilla.png",
                BodyJson = System.Text.Json.JsonSerializer.Serialize(new[]
                {
                    "Saxta parfümlər tez-tez orijinala bənzəyir, amma incə detallarda fərq verir. Bunlara diqqət et:",
                    "## 1. Qutu və selofan",
                    "Orijinal qutular sıx karton, çap dəqiq, selofan tərəfdən bağlı və \"tab\"lı olur. Saxta qutularda selofan adətən səliqəsiz yapışdırılır.",
                    "## 2. Batch kod",
                    "Orijinal flakon və qutunun altında eyni batch kod yazılı olur. checkfresh.com kimi saytlarda kodu yoxlaya bilərsən.",
                    "## 3. Şüşə və sprey",
                    "Orijinal şüşə qalın, ağır, kənarları hamar. Sprey eyni və sıx duman buraxır — saxtalarda sprey \"tüpürür\" və ya zəif çıxır.",
                    "## 4. Qoxunun açılışı",
                    "Saxta ətirlər ilk dəqiqədə güclü olur amma 30 dəqiqədən sonra \"yox olur\". Orijinal ətirlər təbəqələnərək açılır.",
                    "OMAR PERFUME-də hər məhsul rəsmi distribyutordan gəlir — orijinal olmayan heç bir məhsul satışa çıxmır."
                }),
                PublishedAt = new DateTime(2026, 3, 10, 0, 0, 0, DateTimeKind.Utc),
                IsPublished = true
            }
        };
        context.BlogArticles.AddRange(seedArticles);
        await context.SaveChangesAsync();
    }
}

// Global exception handler — bütün exception-lar üçün standart JSON cavab
app.UseMiddleware<GlobalExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseCors("AllowAll");
}
else
{
    app.UseCors("AllowFrontend");
    // HTTPS redirect yalnız production-da — dev mühitdə http qalır
    app.UseHttpsRedirection();
}

// Static files (wwwroot/uploads/...) — admin upload etdiyi şəkillər
app.UseStaticFiles();

app.UseRateLimiter();

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();




app.Run();
