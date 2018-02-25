﻿using SurruhBackend.Models;
using System.Collections.Generic;

namespace SurruhBackend.ViewModels
{
    public class ProductOptionViewModel: BaseEntity
    {
        public int Id { get; set; }
        public double Price { get; set; }

        public IEnumerable<ImageViewModel> Images { get; set; }

        public int ProductId { get; set; }

        public ProductOptionViewModel()
        {
            Images = new List<ImageViewModel>();
        }
    }
}
