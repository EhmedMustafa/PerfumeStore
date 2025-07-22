using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PerfumeStore.Domain.Entities
{
    public class Brand
    {
        public int Id { get; set; }                    // Brendin unikal identifikatoru
        public string Name { get; set; }               // Brendin adı
        public string Tagline { get; set; }
        public string Description { get; set; }
        public string LogoUrl { get; set; }            // Brend loqosunun URL-i

        public ICollection<Product> products { get; set; } // Brendə aid məhsullar
    }
}
