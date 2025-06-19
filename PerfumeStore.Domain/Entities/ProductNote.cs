using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PerfumeStore.Domain.Enums;

namespace PerfumeStore.Domain.Entities
{
    /// Məhsul və Notlar arasında many-to-many əlaqəni təmin edən sinif
    public class ProductNote
    {
        public int ProductId { get; set; }  // Məhsul ID-si
        public int NoteId { get; set; }     // Not ID-si
        public NoteType Type { get; set; }     // Notun növü

        // Navigation Properties

        public Product Product { get; set; }
        public FragranceNote Note { get; set; }

    }
}
