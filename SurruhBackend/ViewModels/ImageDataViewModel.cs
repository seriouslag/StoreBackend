using SurruhBackend.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SurruhBackend.Models
{
    public class ImageDataViewModel
    {
        public ImageDataViewModel()
        {
            Tags = new List<TagViewModel>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Extension { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastModified { get; set; }
        public Boolean IsVisible { get; set; }

        public IEnumerable<TagViewModel> Tags { get; set; }

        public int ImageId { get; set; }
    }
}
