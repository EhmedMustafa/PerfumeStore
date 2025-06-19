using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PerfumeStore.Domain.Entities;

namespace PerfumeStore.Infrastructure.Persistence.Configurations
{
    public class BrandConfiguration : IEntityTypeConfiguration<Brand>

    {
        public void Configure(EntityTypeBuilder<Brand> builder)
        {
            builder.ToTable("Brands");
            
            builder.HasKey(x => x.Id);

            builder.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(p=>p.LogoUrl)
                .HasMaxLength(100);

            // One-to-many əlaqə (Brand -> Products)

            builder.HasMany(b => b.products)        // 1 Brand-in çoxlu Product-u ola bilər
                   .WithOne(p => p.Brand)           // Hər Product yalnız 1 Brand-ə aid ola bilər
                   .HasForeignKey(p => p.BrandId)   // Product cədvəlindəki xarici açar (BrandId)
                   .OnDelete(DeleteBehavior.Restrict); // Brend silinərsə, ona aid məhsullar silinməz

            builder.HasIndex(p => p.Name)
                .IsUnique();
        }
    }
}
