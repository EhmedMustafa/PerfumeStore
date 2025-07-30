 
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client.Extensions.Msal;
using PerfumeStore.Domain.Entities;
using PerfumeStore.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;


namespace PerfumeStore.Infrastructure.Data
{
    public class AppDbContext : IdentityDbContext<AppUser, AppRole, int,
     AppUserClaim, AppUserRole, AppUserLogin, AppRoleClaim, AppUserToken>
    {
        //public AppDbContext()
        //{
                
        //}
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<ProductFeature> ProductFeatures { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        public DbSet<Brand> Brands { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<FragranceFamily> FragranceFamilies { get; set; }
        public DbSet<FragranceNote> FragranceNotes { get; set; }
        public DbSet<ProductNote> ProductNotes { get; set; }

        public DbSet<FragranceNoteType> FragranceNoteTypes { get; set; }

        public DbSet<Cart> Carts { get; set; }

        public DbSet<CartItem> CartItems { get; set; }

        public DbSet<ProductVariant> ProductVariants { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            modelBuilder.Entity<AppUser>(b =>
            {
                b.HasKey(u => u.Id);
            });

            modelBuilder.Entity<AppRole>(b =>
            {
                b.HasKey(r => r.Id);
            });

            modelBuilder.Entity<AppUserRole>(b =>
            {
                b.HasKey(ur => new { ur.UserId, ur.RoleId });
            });

            modelBuilder.Entity<AppUserLogin>(b =>
            {
                b.HasKey(ul => ul.UserId);
            });

            modelBuilder.Entity<AppUserClaim>(b =>
            {
                b.HasKey(uc => uc.Id);
            });

            modelBuilder.Entity<AppRoleClaim>(b =>
            {
                b.HasKey(rc => rc.Id);
            });

            modelBuilder.Entity<AppUserToken>(b =>
            {
                b.HasKey(ut => new { ut.UserId, ut.LoginProvider, ut.Name });
            });

        }
    }
}
