using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SurruhBackend.Models
{
    public class ImageData_Tag
    {
        public int ImageDataId { get; set; }
        public ImageData ImageData { get; set; }

        public int TagId { get; set; }
        public Tag Tag { get; set; }
    }
    
}
