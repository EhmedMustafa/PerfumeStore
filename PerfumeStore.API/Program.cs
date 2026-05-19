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
