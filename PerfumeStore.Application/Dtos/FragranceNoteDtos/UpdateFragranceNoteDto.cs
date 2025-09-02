using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PerfumeStore.Domain.Enums;

namespace PerfumeStore.Application.Dtos.FragranceNoteDtos
{
   public class UpdateFragranceNoteDto
    {
        public int Id { get; set; }             // Hansı not yenilənəcək
        public string Name { get; set; }        // Yeni ad
        public string ImageUrl { get; set; }
        public List<NoteType> Types { get; set; } // Optional: Type dəyişmək üçün
    }
}
