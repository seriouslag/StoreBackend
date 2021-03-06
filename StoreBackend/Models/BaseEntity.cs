﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StoreBackend.Models
{
    public abstract class BaseEntity
    {
        public DateTime? CreatedDate { get; set; }
        public DateTime? LastModified { get; set; }
        public Boolean? IsActivated { get; set; }

        [Required, StringLength(255)]
        public virtual string Name { get; set; }
    }
}
