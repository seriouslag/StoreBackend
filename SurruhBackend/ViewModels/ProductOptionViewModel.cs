using SurruhBackend.Models;
using System.Collections.Generic;

namespace SurruhBackend.ViewModels
{
    public class ProductOptionViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }

        public IEnumerable<ImageDataViewModel> ImageData { get; set; }

        public int ProductId { get; set; }

        public ProductOptionViewModel()
        {
            ImageData = new List<ImageDataViewModel>();
        }
    }
}
