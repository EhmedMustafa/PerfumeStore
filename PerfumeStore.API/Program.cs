﻿using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
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
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "yourdomain.com",
            ValidAudience = "yourdomain.com",
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("YourSuperSecretKey1234567890!@#$%^&*()"))
        };
    });




builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminPolicy", policy => policy.RequireRole("Admin"));
});


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


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
}
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();




app.Run();
