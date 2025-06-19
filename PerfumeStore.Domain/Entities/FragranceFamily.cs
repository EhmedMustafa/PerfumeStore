using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PerfumeStore.Domain.Entities
{
    public class FragranceFamily
    {
        public int Id { get; set; }                    // Ailənin unikal identifikatoru
        public string Name { get; set; }               // Ailənin adı (ingiliscə)
        public ICollection<Product> products { get; set; } // Bu ailəyə aid məhsullar
    }
}
