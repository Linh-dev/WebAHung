using eFashionShop.Application.Categories;
using eFashionShop.ViewModels.Catalog.Categories;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace eFashionShop.Controllers.AdminController
{
    public class AdminCategoryController : AdminBaseController
    {
        private readonly ICategoryService _categoryService;
        public AdminCategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        public async Task<IActionResult> Index()
        {
            var data = await _categoryService.GetAll();
            return View(data);
        }
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var result = _categoryService.Delete(id);
            if (result.IsCompleted)
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
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var ListCategoryParent = await _categoryService.GetListParent();
            ViewBag.ListCategoryParent = ListCategoryParent;
            return View();
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Create([FromForm] CategoryCreateVm res)
        {
            var ListCategoryParent = await _categoryService.GetListParent();
            if (!ModelState.IsValid)
            {
                ViewBag.ListCategoryParent = ListCategoryParent;
                return View(res);
            }

            var result = _categoryService.Create(res);
            if (result.Result)
            {
                TempData["result"] = "Thêm mới thành công";
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("", "Thêm mới thất bại");
            ViewBag.ListCategoryParent = ListCategoryParent;
            return View(res);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var res = await _categoryService.GetByIdForUpdate(id);
            var ListCategoryParent = await _categoryService.GetListParent();
            ViewBag.ListCategoryParent = ListCategoryParent;
            return View(res);
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Edit([FromForm] CategoryUpdateVm res)
        {
            var ListCategoryParent = await _categoryService.GetListParent();
            if (!ModelState.IsValid)
            {
                ViewBag.ListCategoryParent = ListCategoryParent;
                return View(res);
            }

            var result = _categoryService.Edit(res);
            if (result.Result)
            {
                TempData["result"] = "Thêm mới thành công";
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("", "Thêm mới thất bại");
            ViewBag.ListCategoryParent = ListCategoryParent;
            return View(res);
        }
    }
}
