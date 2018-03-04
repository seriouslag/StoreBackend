using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StoreBackend.Models
{
    public class Product_Tag
    {
        public int ProductId { get; set; }
        public Product Product { get; set; }

        public int TagId { get; set; }
        public Tag Tag { get; set; }
    }
    
}
