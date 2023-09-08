using eFashionShop.Data.EF;
using eFashionShop.ViewModels.Catalog.Categories;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using eFashionShop.Exceptions;
using eFashionShop.Data.Entities;
using eFashionShop.Data.Enums;
using eFashionShop.Application.CloudinaryService;

namespace eFashionShop.Application.Categories
{
    public class CategoryService : ICategoryService
    {
        private readonly EShopDbContext _context;
        private readonly IPhotoService _photoService;

        public CategoryService(EShopDbContext context, IPhotoService photoService)
        {
            _context = context;
            _photoService = photoService;
        }

        public async Task<bool> Create(CategoryCreateVm categoryVm)
        {
            if (categoryVm == null) throw new EShopException("Create fail!");
            if(categoryVm.File == null) throw new EShopException("Create fail!");
            var image = await _photoService.AddPhotoAsync(categoryVm.File);
            var category = new Category
            {
                Name = categoryVm.Name,
                IsShowOnHome = categoryVm.IsShowOnHome,
                ParentId = categoryVm.ParentId,
                Status = Status.Active,
                ImagePublishId = image.PublicId,
                ImageUrl = image.SecureUrl.AbsoluteUri
            };
            _context.Categories.Add(category);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> Delete(int id)
        {
            var category = _context.Categories.Find(id);
            if (category == null) throw new EShopException("Delete fail!");
            _context.Categories.Remove(category);
            if (!string.IsNullOrEmpty(category.ImagePublishId)) await _photoService.DeletePhotoAsync(category.ImagePublishId);
            var childCategories = _context.Categories.Where(x => x.ParentId == category.Id);
            if(childCategories.Any())
            {
                var childCategories1 = await childCategories.ToListAsync();
                foreach (var c in childCategories1)
                {
                    _context.Categories.Remove(c);
                    if (!string.IsNullOrEmpty(c.ImagePublishId)) await _photoService.DeletePhotoAsync(c.ImagePublishId);
                }
            }
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> Edit(CategoryUpdateVm categoryUpdateVm)
        {
            var category = _context.Categories.Find(categoryUpdateVm.Id);
            if (category == null) throw new EShopException("Update fail!");
            category.Name = categoryUpdateVm.Name;
            category.IsShowOnHome = categoryUpdateVm.IsShowOnHome;
            category.ParentId = category.ParentId;
            if (categoryUpdateVm.File != null)
            {
                var image = await _photoService.AddPhotoAsync(categoryUpdateVm.File);
                category.ImagePublishId = image.PublicId;
                category.ImageUrl = image.SecureUrl.AbsoluteUri;
            }
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
