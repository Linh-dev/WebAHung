using eFashionShop.ViewModels.Catalog.Images;
using eFashionShop.ViewModels.Catalog.Products;
using eFashionShop.ViewModels.Utilities.Slides;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eFashionShop.ViewModels.WebAppViewModel
{
    public class HomeViewModel
    {

        public List<ProductFeatureVm> FeaturedProducts { get; set; }

        public List<ImageVm> FeaturedImages { get; set; }
    }
}