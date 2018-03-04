using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StoreBackend.Models
{
    public class Product: BaseEntity
    {
        public int Id { get; set; }

        //figure out how to make unique
        [Required]
        public override string Name { get; set; }

        [Required]
        public string ProductDescription { get; set; }

        public IEnumerable<ProductOption> ProductOptions { get; set; }
        public IEnumerable<Product_Tag> Tags { get; set; }

        public Product()
        {
            ProductOptions = new List<ProductOption>();
            Tags = new List<Product_Tag>();
        }

    }
}
