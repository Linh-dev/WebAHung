using eFashionShop.Application.Images;
using eFashionShop.Exceptions;
using eFashionShop.ViewModels.Catalog.Images;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eFashionShop.Controllers.AdminController
{
    public class AdminImageController : AdminBaseController
    {
        private readonly IImageService _imageService;
        private readonly IConfiguration _configuration;

        public AdminImageController(IImageService imageService,
            IConfiguration configuration)
        {
            _configuration = configuration;
            _imageService = imageService;
        }

        public async Task<IActionResult> Index(int? categoryId, int pageIndex = 1, int pageSize = 20)
        {

            var request = new GetManageImagePagingRequest()
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
            };
            var data = await _imageService.GetAll(request);
            return View(data);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> AddListImages([FromForm] List<IFormFile> images)
        {
            if (images.Count < 1) return RedirectToAction("Create");
            try
            {
                var res = new List<ImageCreateRedVm>();
                foreach (var file in images)
                {
                    var image = new ImageCreateRedVm()
                    {
                        Caption = "ảnh tự tạo",
                        File = file,
                        IsFeatured = false,
                    };
                    res.Add(image);
                }
                await _imageService.AddImage(res);
                return RedirectToAction("Index");
            }
            catch(Exception ex)
            {

                return BadRequest(ex);
            }
        }

        [HttpGet]
        public async Task<IActionResult> DeleteImage(int id)
        {
            if (id < 0) throw new EShopException("Delete image fail!");
            var result = await _imageService.DeleteImage(id);
            if (result) return RedirectToAction("Index");
            throw new EShopException("Delete image fail!");
        }
        [HttpGet]
        public async Task<IActionResult> SetFeaturedImage(int id)
        {
            if (id < 0) throw new EShopException("Set main image fail!");
            var result = await _imageService.SetFeaturedImage(id);
            if (result) return RedirectToAction("Index");
            throw new EShopException("Set main image fail!");
        }
    }
}
