﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PerfumeStore.Domain.Entities;

namespace PerfumeStore.Application.Interfaces.IOrderItemRepository
{
    public interface IOrderItemRepository
    {
        Task <List<OrderItem>> GetOrderItemWithDetails();
    }
}
