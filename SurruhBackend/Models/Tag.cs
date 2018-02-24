using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SurruhBackend.Models
{
    public class Tag
    {
        public Tag()
        {
            ImageData_Tags = new List<ImageData_Tag>();
        }

        public int Id { get; set; }
        [Required]
        public string Name { get; set; }

        public ICollection<ImageData_Tag> ImageData_Tags { get; set; }

        
    }
}

