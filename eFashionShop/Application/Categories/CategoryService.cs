using eFashionShop.Application.Images;
using eFashionShop.Data.EF;
using eFashionShop.Data.Entities;
using eFashionShop.Exceptions;
using eFashionShop.Extensions;
using eFashionShop.ViewModels.Catalog.Categories;
using eFashionShop.ViewModels.Catalog.Images;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eFashionShop.Application.Categories
{
    public class CategoryService : ICategoryService
    {
        private readonly EShopDbContext _context;
        private readonly IImageService _imageService;
        public CategoryService(EShopDbContext context, IImageService imageService)
        {
            _context = context;
            _imageService = imageService;
        }

        public async Task<bool> Create(CategoryCreateVm categoryVm)
        {
            if (categoryVm == null) throw new EShopException("Create fail!");
            var category = new Category();
            categoryVm.CopyProperties(category);
            if (categoryVm.File != null)
            {
                var imageFile = new ImageCreateRedVm();
                imageFile.File = categoryVm.File;
                var imageRes = await _imageService.AddImage(imageFile, 0);
                category.ImageId = imageRes.ImageId;
                category.ImageUrl = imageRes.ImageUrl;
                category.ImagePublishId = imageRes.ImagePublishId;

            }
            _context.Categories.Add(category);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> Delete(int id)
        {
            try
            {
                var category = await _context.Categories.FindAsync(id);
                if (category == null) throw new EShopException("Delete fail!");
                //Todo bổ sung delte image cho category
                await _imageService.DeleteImage(category.ImageId);

                var childCategories = await _context.Categories.Where(x => x.ParentId == category.Id).ToListAsync();
                var x = childCategories.Count();
                if (childCategories.Any())
                {
                    foreach (var c in childCategories)
                    {
                        _context.Categories.Remove(c);
                        await _imageService.DeleteImage(c.ImageId);
                    }
                }
                _context.Categories.Remove(category);
                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<bool> Edit(CategoryUpdateVm categoryUpdateVm)
        {
            var category = await _context.Categories.FindAsync(categoryUpdateVm.Id);
            if (category == null) throw new EShopException("Update fail!");
            categoryUpdateVm.CopyProperties(category);

            if (categoryUpdateVm.File != null)
            {
                // xoa anh cu
                await _imageService.DeleteImage(category.ImageId);
                var imageFile = new ImageCreateRedVm();
                imageFile.File = categoryUpdateVm.File;
                var imageRes = await _imageService.AddImage(imageFile, 0);
                category.ImageId = imageRes.ImageId;
                category.ImageUrl = imageRes.ImageUrl;
                category.ImagePublishId = imageRes.ImagePublishId;
            }
            _context.Update(category);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<List<CategoryVm>> GetAll()
        {
            var query = from c in _context.Categories
                        select new { c };
            return await query.Select(x => new CategoryVm()
            {
                Id = x.c.Id,
                Name = x.c.Name,
                ParentId = x.c.ParentId,
                ImagePublishId = x.c.ImagePublishId,
                ImageUrl = x.c.ImageUrl
            }).ToListAsync();
        }

        public async Task<CategoryVm> GetById(int id)
        {
            var query = from c in _context.Categories where c.Id == id  
                        select new { c };
            var res = await query.Select(x => new CategoryVm()
            {
                Id = x.c.Id,
                Name = x.c.Name,
                ParentId = x.c.ParentId,
                ImageUrl = x.c.ImageUrl
            }).FirstOrDefaultAsync();
            return res;
        }

        public async Task<CategoryUpdateVm> GetByIdForUpdate(int id)
        {
            var query = from c in _context.Categories
                        where c.Id == id
                        select new { c };
            var res = await query.Select(x => new CategoryUpdateVm()
            {
                Id = x.c.Id,
                Name = x.c.Name,
                ParentId = x.c.ParentId,
                IsShowOnHome = x.c.IsShowOnHome
            }).FirstOrDefaultAsync();
            return res;
        }

        public async Task<List<CategoryVm>> GetListParent(int id = 0)
        {
            var query = from c in _context.Categories where c.ParentId == -1 && c.Id != id
                        select new { c };
            var x = await query.Select(x => new CategoryVm()
            {
                Id = x.c.Id,
                Name = x.c.Name,
                ParentId = x.c.ParentId
            }).ToListAsync();
            return x;
        }
    }
}
