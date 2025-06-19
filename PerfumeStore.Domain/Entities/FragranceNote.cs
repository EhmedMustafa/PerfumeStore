using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PerfumeStore.Domain.Enums;

namespace PerfumeStore.Domain.Entities
{
    public class FragranceNote
    {
        public int NoteId { get; set; }                    // Notun unikal identifikatoru
        public string Name { get; set; }               // Notun adı (ingiliscə)

        public NoteType Type { get; set; }                // Notun növü (Top, Middle, Base)

        public ICollection<Product> products { get; set; }  // Bu notu olan məhsullar

    }
}
