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
    public class FragranceFamilyConfiguration : IEntityTypeConfiguration<FragranceFamily>
    {
        public void Configure(EntityTypeBuilder<FragranceFamily> builder)
        {
            builder.HasKey(x => x.Id);

            builder.ToTable("FragranceFamilies");

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(50);

            builder.HasMany(p=>p.products)
                .WithOne(b=>b.Family)
                .HasForeignKey(b=>b.FamilyId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(ff => ff.Name)
              .IsUnique(); // Eyni adlı 2 ətri ailə ola bilməz
        }
    }
}
