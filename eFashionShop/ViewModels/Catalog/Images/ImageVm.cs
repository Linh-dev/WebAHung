using System;

namespace eFashionShop.ViewModels.Catalog.Images
{
    public class ImageVm
    {
        public int Id { get; set; }
        public string ImagePath { get; set; }
        public string Caption { get; set; }
        public bool IsFeatured { get; set; }
        public int? SortOder { get; set; }

    }
}
