using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StoreBackend.Models
{
    public abstract class BaseEntityViewModel
    {
        public DateTime? CreatedDate { get; set; }
        public DateTime? LastModified { get; set; }
        public bool? IsActivated { get; set; }

        public string Name { get; set; }
    }
}
