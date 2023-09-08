using Microsoft.AspNetCore.Mvc;

namespace eFashionShop.Controllers
{
    public class BlogController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
