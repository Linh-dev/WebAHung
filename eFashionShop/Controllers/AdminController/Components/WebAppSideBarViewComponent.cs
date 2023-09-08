using eFashionShop.Application.Categories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace eFashionShop.Controllers.Components
{
    public class WebAppSideBarViewComponent : ViewComponent
    {
        private readonly ICategoryService _categoryApiClient;

        public WebAppSideBarViewComponent(ICategoryService categoryApiClient)
        {
            _categoryApiClient = categoryApiClient;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var items = await _categoryApiClient.GetAll();
            return View(items);
        }
    }
}