using Microsoft.AspNetCore.Mvc;

namespace eFashionShop.Controllers
{
    public class ServiceController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
