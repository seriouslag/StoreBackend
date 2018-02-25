using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SurruhBackend.Models
{
    public class Image: BaseEntity
    {
        public Image()
        {
            ProductOption = new List<ProductOption_Image>();
        }

        public int Id { get; set; }
        public byte[] Content { get; set; }

        public int Height { get; set; }
        public int Width { get; set; }

        [Required, StringLength(64)]
        public string ContentType { get; set; }

        public IEnumerable<ProductOption_Image> ProductOption { get; set; }
        
        public string Extension()
        {
            // add one because it adds an underscore if not? idk where the underscore comes from
            return ContentType.Substring(ContentType.IndexOf("/") + 1);
        }

        public string FileName()
        {
            return Name + "." + Extension();
        }
    }
}
