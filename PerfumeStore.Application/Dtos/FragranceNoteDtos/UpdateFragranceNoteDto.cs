using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PerfumeStore.Application.Dtos.FragranceNoteDtos
{
   public class UpdateFragranceNoteDto
    {
        public int Id { get; set; }             // Hansı not yenilənəcək
        public string Name { get; set; }        // Yeni ad
    }
}
