using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SurruhBackend.Models
{
    public class ProductOption_ImageData
    {
        public int ProductOptionId { get; set; }
        public ProductOption ProductOption { get; set; }

        public int ImageDataId { get; set; }
        public ImageData ImageData { get; set; }
    }
}
