using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PerfumeStore.Domain.Enums;

namespace PerfumeStore.Domain.Entities
{
    public class FragranceNoteType //  notlar
    {
        public int FragranceNoteId { get; set; }
        public FragranceNote FragranceNote { get; set; }

        public NoteType Type { get; set; } // Top, Middle, Base

    }
}
