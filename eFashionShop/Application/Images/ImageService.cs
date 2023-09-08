using eFashionShop.Application.CloudinaryService;
using eFashionShop.Constants;
using eFashionShop.Data.EF;
using eFashionShop.Data.Entities;
using eFashionShop.ViewModels.Catalog.Images;
using eFashionShop.ViewModels.Catalog.ProductImages;
using eFashionShop.ViewModels.Catalog.Products;
using eFashionShop.ViewModels.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eFashionShop.Application.Images
{
    public class ImageService : IImageService
    {
        private readonly EShopDbContext _context;
        private readonly IPhotoService _photoServiece;
        private const string USER_CONTENT_FOLDER_NAME = "user-content";


        public ImageService(EShopDbContext context, IPhotoService photoServiece)
        {
            _context = context;
            _photoServiece = photoServiece;
        }
        public async Task<List<ImageVm>> GetFeaturedImages(int count)
        {
            var query = _context.ProductImages.OrderBy(x => x.SortOrder).Where(x => x.IsFeatured == true).AsQueryable();

            if(count > 0)
            {
                query.Take(count);
            }
            var result = await query.Select(x => new ImageVm
            {
                Id = x.Id,
                ImagePath = x.ImagePath,
                Caption = x.Caption,
                IsFeatured = x.IsFeatured,
            }).ToListAsync();

            return result;
        }

        public async Task<bool> SetFeaturedImage(int id)
        {
            var query = await _context.ProductImages.OrderBy(x => x.SortOrder).Where(x => x.SortOrder != 999 && x.Id != id).ToListAsync();
            var image = await _context.ProductImages.FirstOrDefaultAsync(x => x.Id == id);
            image.IsFeatured = true;
            image.SortOrder = 0;
            _context.ProductImages.Update(image);
            for(int i = 0; i < query.Count; i++)
            {
                if(query[i].SortOrder >= 0 && query[i].SortOrder < SystemConstants.ProductSettings.NumberOfFeaturedImages) 
                {
                    query[i].SortOrder += 1;
                }
                else
                {
                    query[i].IsFeatured = false;
                    query[i].SortOrder = 999;
                }
                _context.ProductImages.Update(query[i]);
            }
            return await _context.SaveChangesAsync() > 0;

        }

        public async Task<bool> AddImage(List<ImageCreateRedVm> items)
        {
            try
            {
                foreach(var red in items)
                {
                    var resultImage = await _photoServiece.AddPhotoAsync(red.File);
                    if (resultImage == null) return false;
                    ProductImage image = new ProductImage();
                    image.ImagePath = resultImage.SecureUrl.AbsoluteUri;
                    image.PublicId = resultImage.PublicId;
                    image.Caption = red.Caption;
                    image.FileSize = 0;
                    image.ProductId = 0;
                    image.DateCreated = DateTime.Now;
                    image.IsDefault = false;
                    _context.ProductImages.Add(image);
                }
                return await _context.SaveChangesAsync() > 0;
            }
            catch(Exception ex)
            {
                throw new Exception();
            }

        }

        public async Task<bool> DeleteImage(int id)
        {
            var image = await _context.ProductImages.FirstOrDefaultAsync(x => x.Id == id);
            if (image == null) return false;
            await _photoServiece.DeletePhotoAsync(image.PublicId);

            _context.ProductImages.Remove(image);
            return await _context.SaveChangesAsync() > 0;

        }

        public async Task<PagedResult<ImageVm>> GetAll(GetManageImagePagingRequest request)
        {
            var data = new List<ImageVm>();
            var query = _context.ProductImages.OrderBy(x => x.SortOrder);
            data = await query.Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(x => new ImageVm()
                {
                    Id = x.Id,
                    ImagePath = x.ImagePath,
                    Caption = x.Caption,
                    IsFeatured = x.IsFeatured,
                    SortOder = x.SortOrder
                }).ToListAsync();
            var totalRecords = await query.CountAsync();
            var result = new PagedResult<ImageVm>()
            {
                TotalRecords = totalRecords,
                PageSize = request.PageSize,
                PageIndex = request.PageIndex,
                Items = data
            };
            return result;
        }
    }
}
