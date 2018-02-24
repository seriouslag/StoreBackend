using System;
using System.Collections.Generic;

namespace SurruhBackend.Models
{
    public class ImageData
    {
        public ImageData()
        {
            ImageData_Tags = new List<ImageData_Tag>();
            ProductOption_ImageData = new List<ProductOption_ImageData>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Extension { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastModified { get; set; }
        public Boolean IsVisible { get; set; }

        public IEnumerable<ImageData_Tag> ImageData_Tags { get; set; }

        public IEnumerable<ProductOption_ImageData> ProductOption_ImageData { get; set; }
    
        public int ImageId { get; set; }
        public Image Image { get; set; }

        
    }
}
