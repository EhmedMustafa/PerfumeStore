using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PerfumeStore.Domain.Enums;

namespace PerfumeStore.Domain.Entities
{
    public class FragranceNote //  notlar
    {
        public int Id { get; set; }                         // Not ID
        public string Name { get; set; }                    // Məsələn: Vanilla

        public string ImageUrl { get; set; }

        public ICollection<FragranceNoteType> NoteTypes { get; set; } = new List<FragranceNoteType>();
        public ICollection<ProductNote> ProductNotes { get; set; } = new List<ProductNote>();


    }
}
