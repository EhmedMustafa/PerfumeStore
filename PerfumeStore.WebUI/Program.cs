using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PerfumeStore.Application.Interfaces;
using PerfumeStore.Application.Interfaces.IProductRepository;
using PerfumeStore.Application.Profiles;
using PerfumeStore.Application.Services.FragranceFamilyService;
using PerfumeStore.Application.Services.FragranceNoteServices;
using PerfumeStore.Application.Services.ProductServices;
using PerfumeStore.Infrastructure.Data;
using PerfumeStore.Infrastructure.Repositories;
using PerfumeStore.Infrastructure.Repositories.ProductRepository;
using PerfumeStore.Infrastructure.Services;


var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Add services to the container.
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped(typeof(IProductRepository), typeof(ProductsRepository));
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IFragranceFamilyService, FragranceFamilyService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
