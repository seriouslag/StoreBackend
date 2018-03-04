using StoreBackend.ViewModels;
using System.Collections.Generic;

namespace StoreBackend.Models
{
    public class ProductViewModel: BaseEntity
    {
        public int Id { get; set; }
        public string ProductDescription { get; set; }

        public IEnumerable<ProductOptionViewModel> ProductOptions { get; set; }

        public ProductViewModel()
        {
            ProductOptions = new List<ProductOptionViewModel>();
        }
    }
}
