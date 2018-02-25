using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SurruhBackend.Models
{
    public class ProductOption: BaseEntity
    {

        public ProductOption()
        {
            Images = new List<ProductOption_Image>();
        }


        public int Id { get; set; }
        public double Price { get; set; }
        public string ProductOptionDescription { get; set; }


        public IEnumerable<ProductOption_Image> Images { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }

        
    }
}
