using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client.Extensions.Msal;
using PerfumeStore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PerfumeStore.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            
        }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> categories { get; set; }
    }
}
