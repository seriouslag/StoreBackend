using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SurruhBackend.Models
{
    public abstract class BaseEntity
    {
        public DateTime? CreatedDate { get; set; }
        public DateTime? LastModified { get; set; }
        public Boolean? IsVisible { get; set; }

        [Required, StringLength(255)]
        public string Name { get; set; }
    }
}
