using eFashionShop.Application.Contacts;
using eFashionShop.Data.EF;
using eFashionShop.ViewModels.Catalog.Contacts;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace eFashionShop.Controllers.AdminController
{
    public class AdminContactController : AdminBaseController
    {
        private readonly IContactService _contactService;
        public AdminContactController(IContactService contactService)
        {
            _contactService = contactService;
        }
        public async Task<IActionResult> Index()
        {
            var data = await _contactService.GetAll();
            var res = data;
            return View(res);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ContactCreateVm res)
        {
            if (!ModelState.IsValid) return View(res);
            var result = await _contactService.Create(res);
            if (result)
            {
                TempData["result"] = "Thêm mới thành công";
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("", "Thêm mới thất bại");
            return View(res);
        }
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var result = _contactService.Delete(id);
            if(result.IsCompleted)
            {
                TempData["result"] = "Xoá Thành công";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["result"] = "Xoá thất bại";
                return RedirectToAction("Index");
            }
        }


    }
}
