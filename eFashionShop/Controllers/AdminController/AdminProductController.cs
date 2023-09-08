using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eFashionShop.Application.Categories;
using eFashionShop.Application.Products;
using eFashionShop.Constants;
using eFashionShop.Exceptions;
using eFashionShop.ViewModels.Catalog.ProductImages;
using eFashionShop.ViewModels.Catalog.Products;
using eFashionShop.ViewModels.Common;
using eFashionShop.ViewModels.System.Products;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;

namespace eFashionShop.Controllers.AdminController
{
    public class AdminProductController : AdminBaseController
    {
        private readonly IProductService _productService;
        private readonly IConfiguration _configuration;

        private readonly ICategoryService _categoryService;

        public AdminProductController(IProductService productApiClient,
            IConfiguration configuration,
            ICategoryService categoryApiClient)
        {
            _configuration = configuration;
            _productService = productApiClient;
            _categoryService = categoryApiClient;
        }

        public async Task<IActionResult> Index(string keyword, int? categoryId, int pageIndex = 1, int pageSize = 10)
        {
            var request = new GetManageProductPagingRequest()
            {
                Keyword = keyword,
                PageIndex = pageIndex,
                PageSize = pageSize,
                CategoryId = categoryId
            };
            var data = await _productService.GetAllPaging(request);
            ViewBag.Keyword = keyword;

            var categories = await _categoryService.GetAll();
            ViewBag.Categories = categories.Select(x => new SelectListItem()
            {
                Text = x.Name,
                Value = x.Id.ToString(),
                Selected = categoryId.HasValue && categoryId.Value == x.Id
            });

            if (TempData["result"] != null)
            {
                ViewBag.SuccessMsg = TempData["result"];
            }
            return View(data);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Create([FromForm] ProductCreateRequest request)
        {
            if (!ModelState.IsValid)
                return View(request);

            var result = await _productService.Create(request);
            if (result != 0)
            {
                TempData["result"] = "Thêm mới sản phẩm thành công";
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "Thêm sản phẩm thất bại");
            return View(request);
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> AddListImages([FromForm] List<IFormFile> images, int productId)
        {
            if (images.Count < 1) return RedirectToAction("Edit", new { id = productId });
            var res = new ImagesCreateVm()
            {
                ImageFiles = images,
                ProductId = productId
            };
            var result = await _productService.AddListImages(res);
            if (result > 0) return RedirectToAction("Edit", new { id = productId });
            throw new EShopException("Upload fail!");
        }

        [HttpGet]
        public async Task<IActionResult> CategoryAssign(int id)
        {
            var roleAssignRequest = await GetCategoryAssignRequest(id);
            return View(roleAssignRequest);
        }

        [HttpPost]
        public async Task<IActionResult> CategoryAssign(CategoryAssignRequest request)
        {
            if (!ModelState.IsValid)
                return View();

            var result = await _productService.CategoryAssign(request.Id, request);

            if (result.IsSuccessed)
            {
                TempData["result"] = "Cập nhật danh mục thành công";
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", result.Message);
            var roleAssignRequest = await GetCategoryAssignRequest(request.Id);

            return View(roleAssignRequest);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            if(id == 0) return View();
            var product = await _productService.GetById(id);
            var images = await _productService.GetListImages(id);
            var editVm = new ProductUpdateRequest()
            {
                Id = product.Id,
                Description = product.Description,
                Details = product.Details,
                Name = product.Name,
                Customer = product.Customer,
                Localtion = product.Localtion,
                Area = product.Area,
                IsFeatured = product.IsFeatured
            };
            ViewBag.images = images;
            return View(editVm);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ProductUpdateRequest request)
        {
            var images = await _productService.GetListImages(request.Id);
            if (!ModelState.IsValid)
            {
                ViewBag.images = images;
                return View(request);
            }

            var result = await _productService.Update(request);
            if (result !=0)
            {
                TempData["result"] = "Cập nhật sản phẩm thành công";
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "Cập nhật sản phẩm thất bại");
            ViewBag.images = images;
            return View(request);
        }

        private async Task<CategoryAssignRequest> GetCategoryAssignRequest(int id)
        {
            var productObj = await _productService.GetById(id);
            var categories = await _categoryService.GetAll();
            var categoryAssignRequest = new CategoryAssignRequest();
            foreach (var role in categories)
            {
                categoryAssignRequest.Categories.Add(new SelectItem()
                {
                    Id = role.Id.ToString(),
                    Name = role.Name,
                    Selected = productObj.Categories.Contains(role.Id)
                });
            }
            return categoryAssignRequest;
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            return View(new ProductDeleteRequest()
            {
                Id = id
            });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(ProductDeleteRequest request)
        {
            if (!ModelState.IsValid)
                return View();

            var result = await _productService.Delete(request.Id);
            if (result !=0)
            {
                TempData["result"] = "Xóa sản phẩm thành công";
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "Xóa không thành công");
            return View(request);
        }
        [HttpGet]
        public async Task<IActionResult> DeleteImage(int id, int productImageId)
        {
            if (id < 0) throw new EShopException("Delete image fail!");
            var result = await _productService.RemoveImage(id);
            if (result > 0) return RedirectToAction("Edit", new { id = productImageId });
            throw new EShopException("Delete image fail!");
        }
        [HttpGet]
        public async Task<IActionResult> SetMainImage(int id, int productImageId)
        {
            if (id < 0) throw new EShopException("Set main image fail!");
            var result = await _productService.SetMainImage(id, productImageId);
            if (result > 0) return RedirectToAction("Edit", new { id = productImageId });
            throw new EShopException("Set main image fail!");
        }
    }
}