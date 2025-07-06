using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PerfumeStore.Domain.Enums;

namespace PerfumeStore.Application.Dtos.ProductDtos
{
   public class ProductNoteDto
    {
        public int FragranceNoteId { get; set; }
        public string FragranceNoteName { get; set; }
        public NoteType Type { get; set; }  // Enum: Top, Middle, Base
    }
}
