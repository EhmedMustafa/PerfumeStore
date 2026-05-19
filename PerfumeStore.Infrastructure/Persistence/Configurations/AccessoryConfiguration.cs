using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PerfumeStore.Domain.Entities;

namespace PerfumeStore.Infrastructure.Persistence.Configurations
{
    public class AccessoryConfiguration : IEntityTypeConfiguration<Accessory>
    {
        public void Configure(EntityTypeBuilder<Accessory> b)
        {
            b.ToTable("Accessories");
            b.HasKey(a => a.Id);
            b.Property(a => a.Name).IsRequired().HasMaxLength(255);
            b.Property(a => a.Description).HasColumnType("nvarchar(max)");
            b.Property(a => a.ImageUrl).HasColumnType("nvarchar(1000)");
            b.Property(a => a.Type).IsRequired().HasMaxLength(32);

            b.HasMany(a => a.Variants)
                .WithOne(v => v.Accessory)
                .HasForeignKey(v => v.AccessoryId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }

    public class AccessoryVariantConfiguration : IEntityTypeConfiguration<AccessoryVariant>
    {
        public void Configure(EntityTypeBuilder<AccessoryVariant> b)
        {
            b.ToTable("AccessoryVariants");
            b.HasKey(v => v.Id);
            b.Property(v => v.Size).IsRequired().HasMaxLength(64);
            b.Property(v => v.Price).IsRequired().HasColumnType("decimal(18,2)");
        }
    }
}
