using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SurruhBackend.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ProductDescription { get; set; }

        public IEnumerable<ProductOption> ProductOptions { get; set; }

        public Product()
        {
            ProductOptions = new List<ProductOption>();
        }
    }
}
