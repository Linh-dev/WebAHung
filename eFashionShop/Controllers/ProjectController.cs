using eFashionShop.Application.Products;
using eFashionShop.Constants;
using eFashionShop.ViewModels.WebAppViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace eFashionShop.Controllers
{
    public class ProjectController : Controller
    {
        private readonly ILogger<CategoryController> _logger;
        private readonly IProductService _productService;

        public ProjectController(ILogger<CategoryController> logger,
            IProductService productService)
        {
            _logger = logger;
            _productService = productService;
        }
        public async Task<IActionResult> Index()
        {
            var data = await _productService.GetAll();
            return View(data);
        }
        [HttpGet]
        public async Task<IActionResult> Deital(int id)
        {
            var res = new ProductDetailViewModel()
            {
                FeaturedProducts = await _productService.GetFeaturedProducts(SystemConstants.ProductSettings.NumberOfFeaturedProducts),
                productImageViewModels = await _productService.GetListImages(id),
                productVms = await _productService.GetById(id)

            };
            return View(res);
        }
    }
}
