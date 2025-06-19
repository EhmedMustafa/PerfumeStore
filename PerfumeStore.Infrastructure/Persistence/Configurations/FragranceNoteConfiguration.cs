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
    public class FragranceNoteConfiguration : IEntityTypeConfiguration<FragranceNote>
    {
        public void Configure(EntityTypeBuilder<FragranceNote> builder)
        {
            builder.HasKey(p => p.NoteId);

            builder.ToTable("FragranceNote");

            builder.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(p => p.Type)
                .IsRequired()
                .HasConversion<string>();       // Enum'u string kimi saxlayır (SQL-də "Top", "Middle", "Base")

            // 3. Çoxa çox əlaqə (FragranceNote ↔ Product)

            builder.HasMany(p => p.products)
                .WithMany(b => b.notes)
                .UsingEntity<Dictionary<string, object>>(
                "ProductFragranceNotes",
                j => j.HasOne<Product>().WithMany().HasForeignKey("ProductId"),
                j => j.HasOne<FragranceNote>().WithMany().HasForeignKey("NoteId")
                );

            builder.HasIndex(p => p.Name)
                .IsUnique();

                
        }
    }
}
