﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PerfumeStore.Domain.Entities;

namespace PerfumeStore.Application.Dtos.CartItemDtos
{
    public class CreateCartItemDto
    {
     

      //  public int CartId { get; set; }
       

       // public int ProductId { get; set; }

        public int ProductVariantId { get; set; }

        public int Quantity { get; set; }

        
        //public decimal TotalPrice { get; set; } serverde
    }
}
