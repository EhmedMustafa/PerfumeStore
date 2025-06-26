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
            builder.HasKey(p => p.Id);

            builder.ToTable("FragranceNote");

            builder.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(100);

           

            // 3. Çoxa çox əlaqə (FragranceNote ↔ Product)

            

            builder.HasIndex(p => p.Name).IsUnique();


            builder.HasMany(fn => fn.NoteTypes)
             .WithOne(nt => nt.FragranceNote)
             .HasForeignKey(nt => nt.FragranceNoteId)
             .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(fn => fn.ProductNotes)
             .WithOne(pn => pn.FragranceNote)
             .HasForeignKey(pn => pn.FragranceNoteId);



        }
    }
}
