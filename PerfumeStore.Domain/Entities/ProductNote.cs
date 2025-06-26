using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PerfumeStore.Domain.Enums;

namespace PerfumeStore.Domain.Entities
{
    public class ProductNote
    {
        public int ProductId { get; set; }  // Məhsul ID-si
        public int FragranceNoteId { get; set; }     // Not ID-si
        public NoteType Type { get; set; }     // Notun növü

        // Navigation Properties

        public Product Product { get; set; }
        public FragranceNote FragranceNote { get; set; }

    }
}
