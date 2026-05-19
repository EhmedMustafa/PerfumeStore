using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PerfumeStore.Domain.Entities;

namespace PerfumeStore.Infrastructure.Persistence.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("Products");

            builder.HasKey(p => p.ProductId);

            builder.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(255);

            // Description uzun ola bilər (məhsul təsviri) — nvarchar(MAX)
            builder.Property(p => p.Description)
                .IsRequired()
                .HasColumnType("nvarchar(max)");

            // ImageUrl da uzun ola bilər (CDN URL-ləri)
            builder.Property(p => p.ImageUrl)
                .HasColumnType("nvarchar(1000)");

            // Gallery images — JSON array, böyük ola bilər
            builder.Property(p => p.GalleryImagesJson)
                .HasColumnType("nvarchar(max)");

            builder.Property(p => p.IsNew)
              .HasDefaultValue(false);

            builder.Property(p => p.IsBestseller)
              .HasDefaultValue(false);

            builder.Property(p => p.Disclaimer)
            .HasMaxLength(200);

            // Foreign Key və Navigation Property əlaqələri

            builder.HasOne(p=>p.Brand)
                .WithMany(b=>b.products)
                .HasForeignKey(p => p.BrandId)
                .OnDelete(DeleteBehavior.Restrict);  // Brend silinərsə məhsul silinməsin

            builder.HasOne(p=>p.Family)
                .WithMany(b=>b.products)
                .HasForeignKey(p=> p.FamilyId)
                .OnDelete(DeleteBehavior.Restrict);   // Brend silinərsə məhsul silinməsin

            builder.HasOne(p=>p.Category)
                .WithMany(b=>b.Products)
                .HasForeignKey(p=>p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);     // Brend silinərsə məhsul silinməsin

            // Çoxa çox əlaqə (Product və FragranceNote)

           builder.HasMany(p=>p.ProductNotes)
                  .WithOne(pn=>pn.Product)
                  .HasForeignKey(pn=>pn.ProductId)
                  .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
