using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SurruhBackend.Models
{
    public class ProductOption
    {

        public ProductOption()
        {
            ProductOption_ImageData = new List<ProductOption_ImageData>();
        }


        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }

        public IEnumerable<ProductOption_ImageData> ProductOption_ImageData { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }

        
    }
}
