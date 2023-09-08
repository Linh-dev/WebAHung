using eFashionShop.Application.Categories;
using eFashionShop.Application.Images;
using eFashionShop.Application.Products;
using eFashionShop.Constants;
using eFashionShop.ViewModels.Catalog.Products;
using eFashionShop.ViewModels.WebAppViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;

namespace eFashionShop.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ILogger<CategoryController> _logger;
        private readonly IImageService _imageService;
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;

        public CategoryController(ILogger<CategoryController> logger,
            IImageService imageService,
            IProductService productService,
            ICategoryService categoryApiClient)
        {
            _logger = logger;
            _imageService = imageService;
            _productService = productService;
            _categoryService = categoryApiClient;
        }
        public async Task<IActionResult> Index()
        {
            var data = await _categoryService.GetAll();
            return View(data);
        }

        [HttpGet]
        public async Task<IActionResult> Deital(int id)
        {
            var res = await _productService.GetAllByCategoriesId(id);
            var name = "Danh sách";
            var categories = await _categoryService.GetById(id);
            if (categories != null) name = categories.Name;
            ViewBag.name = name;
            return View(res);
        }
    }
}
