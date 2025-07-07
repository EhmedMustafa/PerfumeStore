using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PerfumeStore.Domain.Entities;
//using Stripe.Climate;

namespace PerfumeStore.Application.Interfaces.IOrderRepository
{
    public interface IOrderRepository
    {
        Task <Order> GetOrderWithOrderItems(int id);
    }
}
