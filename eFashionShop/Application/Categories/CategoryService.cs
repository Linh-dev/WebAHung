using eFashionShop.Application.Images;
using eFashionShop.Data.EF;
using eFashionShop.Data.Entities;
using eFashionShop.Exceptions;
using eFashionShop.Extensions;
using eFashionShop.ViewModels.Catalog.Categories;
using Microsoft.EntityFrameworkCore;
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
                //Todo bổ sung add image cho category
            }
            _context.Categories.Add(category);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> Delete(int id)
        {
            var category = _context.Categories.Find(id);
            if (category == null) throw new EShopException("Delete fail!");
            _context.Categories.Remove(category);
            //Todo bổ sung delte image cho category
            //if (!string.IsNullOrEmpty(category.ImagePublishId)) await _photoService.DeletePhotoAsync(category.ImagePublishId);
            var childCategories = _context.Categories.Where(x => x.ParentId == category.Id);
            if (childCategories.Any())
            {
                var childCategories1 = await childCategories.ToListAsync();
                foreach (var c in childCategories1)
                {
                    _context.Categories.Remove(c);
                    //Todo bổ sung delte image cho category
                    //if (!string.IsNullOrEmpty(c.ImagePublishId)) await _photoService.DeletePhotoAsync(c.ImagePublishId);
                }
            }
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> Edit(CategoryUpdateVm categoryUpdateVm)
        {
            var category = _context.Categories.Find(categoryUpdateVm.Id);
            if (category == null) throw new EShopException("Update fail!");
            categoryUpdateVm.CopyProperties(category);

            if (categoryUpdateVm.File != null)
            {
                //Todo bổ sung add image cho category
                //var image = await _photoService.AddPhotoAsync(categoryUpdateVm.File);
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

        public async Task<List<CategoryVm>> GetListParent()
        {
            var query = from c in _context.Categories where c.ParentId == -1
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
