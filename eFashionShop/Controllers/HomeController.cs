using eFashionShop.Application.Images;
using eFashionShop.Application.Products;
using eFashionShop.Application.Slides;
using eFashionShop.Constants;
using eFashionShop.Models;
using eFashionShop.ViewModels.WebAppViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace eFashionShop.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ISlideService _slideService;
        private readonly IProductService _productService;
        private readonly IImageService _imageService;

        public HomeController(ILogger<HomeController> logger,
            ISlideService slideService,
            IProductService productService,
            IImageService imageService)
        {
            _logger = logger;
            _slideService = slideService;
            _productService = productService;
            _imageService = imageService;
        }

        public async Task<IActionResult> Index()
        {
            var viewModel = new HomeViewModel
            {
                FeaturedProducts = await _productService.GetFeaturedProducts(SystemConstants.ProductSettings.NumberOfFeaturedProducts),
                FeaturedImages = await _imageService.GetFeaturedImages(SystemConstants.ProductSettings.NumberOfFeaturedImages),
            };
            return View(viewModel);
        }

        public IActionResult SetCultureCookie(string cltr, string returnUrl)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(cltr)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
                );

            return LocalRedirect(returnUrl);
        }
    }
}
