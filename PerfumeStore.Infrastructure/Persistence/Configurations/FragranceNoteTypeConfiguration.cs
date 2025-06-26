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
    public class FragranceNoteTypeConfiguration : IEntityTypeConfiguration<FragranceNoteType>
    {
        public void Configure(EntityTypeBuilder<FragranceNoteType> builder)
        {
            builder.ToTable("FragranceNoteTypes");

            builder.HasKey(f =>  new {f.FragranceNoteId,f.Type });

            builder.HasOne(fn => fn.FragranceNote)
                .WithMany(fn => fn.NoteTypes)
                .HasForeignKey(fn => fn.FragranceNoteId);


        }
    }
}
