using eFashionShop.ViewModels.Catalog.Images;
using eFashionShop.ViewModels.Catalog.Products;
using System.Collections.Generic;

namespace eFashionShop.ViewModels.WebAppViewModel
{
    public class IntroduceViewModel
    {
        public List<ProductFeatureVm> FeaturedProducts { get; set; }
        public List<ImageVm> FeaturedImages { get; set; }
    }
}
