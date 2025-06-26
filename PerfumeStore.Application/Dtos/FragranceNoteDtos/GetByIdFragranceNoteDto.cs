using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PerfumeStore.Domain.Entities;
using PerfumeStore.Domain.Enums;

namespace PerfumeStore.Application.Dtos.FragranceNoteDtos
{
    public class GetByIdFragranceNoteDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<NoteType> Types { get; set; }

        public ICollection<Product> Products { get; set; }  // Bu not hansı məhsullardadır
    }
}
