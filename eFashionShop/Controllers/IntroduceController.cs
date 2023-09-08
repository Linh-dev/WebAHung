using eFashionShop.Application.Images;
using eFashionShop.Application.Products;
using eFashionShop.Constants;
using eFashionShop.ViewModels.WebAppViewModel;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace eFashionShop.Controllers
{
    public class IntroduceController : Controller
    {
        private readonly IImageService _imageService;
        private readonly IProductService _productService;

        public IntroduceController(IImageService imageService, IProductService productService)
        {
            _imageService = imageService;
            _productService = productService;
        }
        public async Task<IActionResult> Index()
        {
            var FeaturedImages = await _imageService.GetFeaturedImages(SystemConstants.ProductSettings.NumberOfFeaturedImages);
            var FeaturedProducts = await _productService.GetFeaturedProducts(SystemConstants.ProductSettings.NumberOfFeaturedProducts);
            var data = new IntroduceViewModel
            {
                FeaturedImages = FeaturedImages,
                FeaturedProducts = FeaturedProducts
            };
            return View(data);
        }
    }
}
