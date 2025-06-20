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
    public class ProductNoteConfiguration : IEntityTypeConfiguration<ProductNote>
    {
        public void Configure(EntityTypeBuilder<ProductNote> builder)
        {
            builder.ToTable("ProductNotes");

            builder.HasKey(pn => new { pn.ProductId, pn.NoteId, pn.Type });

            builder.Property(p => p.Type)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.HasOne(pn => pn.Product)
                .WithMany(p=>p.ProductNotes)
                .HasForeignKey(pn =>pn.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(pn=>pn.Note)
                .WithMany(p=>p.ProductNotes)
                .HasForeignKey(pn => pn.NoteId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
