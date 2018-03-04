using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StoreBackend.Models
{
    public class ProductOption_Image
    {
        public int ProductOptionId { get; set; }
        public ProductOption ProductOption { get; set; }

        public int ImageId { get; set; }
        public Image Image { get; set; }
    }
}
