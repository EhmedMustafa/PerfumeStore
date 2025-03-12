using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PerfumeStore.Domain.Entities
{
    public class ProductFeature
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string FeatureName { get; set; } = string.Empty;
        public string FeatureValue { get; set; } = string.Empty;

        public Product Product { get; set; }
    }
}
