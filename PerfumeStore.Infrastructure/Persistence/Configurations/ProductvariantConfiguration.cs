using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PerfumeStore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PerfumeStore.Infrastructure.Persistence.Configurations
{
    public class ProductvariantConfiguration : IEntityTypeConfiguration<ProductVariant>
    {
        public void Configure(EntityTypeBuilder<ProductVariant> builder)
        {
            builder.ToTable("ProductVariants");

            builder.HasKey(pv => pv.Id);

            builder.Property(pv => pv.Size)
                .IsRequired();

            builder.Property(px=>px.CurrentPrice)
                .IsRequired();

            builder.HasOne(pv=>pv.Product)
                .WithMany(p=>p.ProductVariants)
                .HasForeignKey(pv => pv.ProductId)
                .OnDelete(DeleteBehavior.Cascade);
            
        }
    }
}
