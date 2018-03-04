using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace StoreBackend.Models
{
    public class Tag: BaseEntity
    {
        public Tag()
        {
            Products = new List<Product_Tag>();
        }

        public int Id { get; set; }

        public ICollection<Product_Tag> Products { get; set; }

        
    }
}

