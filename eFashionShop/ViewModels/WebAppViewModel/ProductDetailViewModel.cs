using eFashionShop.ViewModels.Catalog.Categories;
using eFashionShop.ViewModels.Catalog.Images;
using eFashionShop.ViewModels.Catalog.ProductImages;
using eFashionShop.ViewModels.Catalog.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eFashionShop.ViewModels.WebAppViewModel
{
    public class ProductDetailViewModel
    {
        public List<ProductImageViewModel> productImageViewModels { get; set; }
        public ProductVm productVms { get; set; }
        public List<ProductFeatureVm> FeaturedProducts { get; set; }
    }
}