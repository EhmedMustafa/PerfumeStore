using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;

namespace PerfumeStore.Domain.Entities
{
    public class User: IdentityUser
    {
        public string FullName { get; set; }
        public string Address { get; set; }
        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
