using eFashionShop.Application.CloudinaryService;
using eFashionShop.Application.Images;
using eFashionShop.Data.EF;
using eFashionShop.Data.Entities;
using eFashionShop.Exceptions;
using eFashionShop.ViewModels.Catalog.ProductImages;
using eFashionShop.ViewModels.Catalog.Products;
using eFashionShop.ViewModels.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace eFashionShop.Application.Products
{
    public class ProductService : IProductService
    {
        private readonly EShopDbContext _context;
        private readonly IPhotoService _photoService;
        private const string USER_CONTENT_FOLDER_NAME = "user-content";

        public ProductService(EShopDbContext context, IPhotoService photoService)
        {
            _context = context;
            _photoService = photoService;
        }

        public async Task<int> AddImage(int productId, ProductImageCreateRequest request)
        {
            var productImage = new ProductImage()
            {
                Caption = request.Caption,
                DateCreated = DateTime.Now,
                IsDefault = request.IsDefault,
                ProductId = productId,
                SortOrder = request.SortOrder
            };

            if (request.ImageFile != null)
            {
                var resultImage = await _photoService.AddPhotoAsync(request.ImageFile);
                productImage.ImagePath = resultImage.SecureUrl.AbsoluteUri;
                productImage.PublicId = resultImage.PublicId;
                productImage.FileSize = request.ImageFile.Length;
            }
            _context.ProductImages.Add(productImage);
            await _context.SaveChangesAsync();
            return productImage.Id;
        }

        public async Task<int> Create(ProductCreateRequest request)
        {
            var product = new Product()
            {
                DateCreated = DateTime.Now,
                Name = request.Name,
                Description = request.Description,
                Details = request.Details,
                Customer = request.Customer,
                Localtion = request.Localtion,
                Area = request.Area,
                IsFeatured = request.IsFeatured,
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            //Save image
            if (request.ThumbnailImage != null)
            {
                var resultImage = await _photoService.AddPhotoAsync(request.ThumbnailImage);
                var image = new ProductImage()
                {
                    ProductId = product.Id,
                    Caption = "Thumbnail image",
                    DateCreated = DateTime.Now,
                    FileSize = request.ThumbnailImage.Length,
                    ImagePath = resultImage.SecureUrl.AbsoluteUri,
                    PublicId = resultImage.PublicId,
                    IsDefault = true,
                    SortOrder = -1
                };
                _context.ProductImages.Add(image);
                await _context.SaveChangesAsync();
            }
            return product.Id;
        }

        public async Task<int> Delete(int productId)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null) throw new EShopException($"Cannot find a product: {productId}");

            var images = _context.ProductImages.Where(i => i.ProductId == productId);
            foreach (var image in images)
            {
                await _photoService.DeletePhotoAsync(image.PublicId);
                _context.ProductImages.Remove(image);
            }

            _context.Products.Remove(product);

            return await _context.SaveChangesAsync();
        }

        public async Task<PagedResult<ProductVm>> GetAllPaging(GetManageProductPagingRequest request)
        {
            var data = new List<ProductVm>();
            int totalRow = 0;
            if (request.CategoryId != null && request.CategoryId != 0)
            {
                //1. Select join
                var query = from p in _context.Products
                            join pic in _context.ProductInCategories on p.Id equals pic.ProductId into ppic
                            from pic in ppic.DefaultIfEmpty()
                            join c in _context.Categories on pic.CategoryId equals c.Id into picc
                            from c in picc.DefaultIfEmpty()
                            select new { p, pic };
                //2. filter
                query = query.Where(p => p.pic.CategoryId == request.CategoryId);
                if (!string.IsNullOrEmpty(request.Keyword))
                    query = query.Where(x => x.p.Name.Contains(request.Keyword));

                //3. Paging
                totalRow = await query.CountAsync();

                data = await query.Skip((request.PageIndex - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .Select(x => new ProductVm()
                    {
                        Id = x.p.Id,
                        Name = x.p.Name,
                        DateCreated = x.p.DateCreated,
                        Description = x.p.Description,
                        Details = x.p.Details,
                        Customer = x.p.Customer,
                        Localtion = x.p.Localtion,
                        Area = x.p.Area
                    }).ToListAsync();
            }
            else
            {
                //1. Select join
                var query = from p in _context.Products
                            select new { p };
                //2. filter
                if (!string.IsNullOrEmpty(request.Keyword))
                    query = query.Where(x => x.p.Name.Contains(request.Keyword));

                //3. Paging
                totalRow = await query.CountAsync();

                data = await query.Skip((request.PageIndex - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .Select(x => new ProductVm()
                    {
                        Id = x.p.Id,
                        Name = x.p.Name,
                        DateCreated = x.p.DateCreated,
                        Description = x.p.Description,
                        Details = x.p.Details,
                        Customer = x.p.Customer,
                        Localtion = x.p.Localtion,
                        Area = x.p.Area,
                    }).ToListAsync();
            }


            //4. Select and projection
            var pagedResult = new PagedResult<ProductVm>()
            {
                TotalRecords = totalRow,
                PageSize = request.PageSize,
                PageIndex = request.PageIndex,
                Items = data
            };
            return pagedResult;
        }
        public async Task<List<ProductVm>> GetAll()
        {
            var data = new List<ProductVm>();
            //1. Select join
            var query = from p in _context.Products
                        from i in _context.ProductImages where (i.ProductId == p.Id && i.IsDefault == true)
                        select new { p, i };

            //3. Paging
            data = await query.Select(x => new ProductVm()
                {
                    Id = x.p.Id,
                    Name = x.p.Name,
                    DateCreated = x.p.DateCreated,
                    Description = x.p.Description,
                    Details = x.p.Details,
                    Customer = x.p.Customer,
                    Localtion = x.p.Localtion,
                    ThumbnailImage = x.i.ImagePath,
                    Area = x.p.Area,
                }).ToListAsync();
            return data;
        }
        public async Task<ProductVm> GetById(int productId)
        {
            var product = await _context.Products.FindAsync(productId);

            var categories = await _context.ProductInCategories.Where(x => x.ProductId == productId).Select(x => x.CategoryId).ToListAsync();

            var image = await _context.ProductImages.Where(x => x.ProductId == productId && x.IsDefault == true).FirstOrDefaultAsync();

            var productViewModel = new ProductVm()
            {
                Id = product.Id,
                DateCreated = product.DateCreated,
                Description = product != null ? product.Description : null,
                Details = product != null ? product.Details : null,
                Name = product != null ? product.Name : null,
                Customer = product != null ? product.Customer : null,
                Localtion = product != null ? product.Localtion : null,
                Area = product != null ? product.Area : 0,
                ThumbnailImage = image != null ? image.ImagePath : "no-image.jpg",
                IsFeatured = product.IsFeatured,
                Categories = categories
            };
            return productViewModel;
        }
        public async Task<List<ProductVm>> GetAllByCategoriesId(int categoriesId)
        {
            var data = new List<ProductVm>();
            //list produc
            var producs = await _context.ProductInCategories.Where(x => x.CategoryId == categoriesId).Select(x => x.Product).ToListAsync();

            if(producs.Any())
            {
                foreach( Product product in producs)
                {
                    var image = await _context.ProductImages.Where(i => i.ProductId == product.Id && i.IsDefault == true).FirstOrDefaultAsync();
                    data.Add(new ProductVm()
                    {
                        Id = product.Id,
                        Name = product.Name,
                        DateCreated = product.DateCreated,
                        Description = product.Description,
                        Details = product.Details,
                        Customer = product.Customer,
                        Localtion = product.Localtion,
                        ThumbnailImage = image != null ? image.ImagePath : "",
                        Area = product.Area,
                    });
                }
            }
            return data;
        }

        public async Task<ProductImageViewModel> GetImageById(int imageId)
        {
            var image = await _context.ProductImages.FindAsync(imageId);
            if (image == null)
                throw new EShopException($"Cannot find an image with id {imageId}");

            var viewModel = new ProductImageViewModel()
            {
                Caption = image.Caption,
                DateCreated = image.DateCreated,
                FileSize = image.FileSize,
                Id = image.Id,
                ImagePath = image.ImagePath,
                IsDefault = image.IsDefault,
                ProductId = image.ProductId,
                SortOrder = image.SortOrder
            };
            return viewModel;
        }

        public async Task<List<ProductImageViewModel>> GetListImages(int productId)
        {
            return await _context.ProductImages.Where(x => x.ProductId == productId)
                .Select(i => new ProductImageViewModel()
                {
                    Caption = i.Caption,
                    DateCreated = i.DateCreated,
                    FileSize = i.FileSize,
                    Id = i.Id,
                    ImagePath = i.ImagePath,
                    IsDefault = i.IsDefault,
                    ProductId = i.ProductId,
                    SortOrder = i.SortOrder
                }).ToListAsync();
        }

        public async Task<int> RemoveImage(int imageId)
        {
            var productImage = await _context.ProductImages.FindAsync(imageId);
            if (productImage == null)
                throw new EShopException($"Cannot find an image with id {imageId}");
            _context.ProductImages.Remove(productImage);
            var result = await _context.SaveChangesAsync();
            if (result > 0)
            {
                await _photoService.DeletePhotoAsync(productImage.PublicId);
                return result;
            };
            throw new EShopException($"Cannot find an image with id {imageId}");
        }

        public async Task<int> Update(ProductUpdateRequest request)
        {
            var product = await _context.Products.FindAsync(request.Id);

            if (product == null) throw new EShopException($"Cannot find a product with id: {request.Id}");

            product.Name = request.Name;
            product.Description = request.Description;
            product.Details = request.Details;
            product.Customer = request.Customer;
            product.Localtion = request.Localtion;
            product.Area = request.Area;
            product.IsFeatured = request.IsFeatured;
            _context.Update(product);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> UpdateImage(int imageId, ProductImageUpdateRequest request)
        {
            var productImage = await _context.ProductImages.FindAsync(imageId);
            if (productImage == null)
                throw new EShopException($"Cannot find an image with id {imageId}");

            if (request.ImageFile != null)
            {
                var resultImage = await _photoService.AddPhotoAsync(request.ImageFile);
                productImage.ImagePath = resultImage.SecureUrl.AbsoluteUri;
                productImage.PublicId = resultImage.PublicId;
                productImage.FileSize = request.ImageFile.Length;
            }
            _context.ProductImages.Update(productImage);
            return await _context.SaveChangesAsync();
        }

        public async Task<ApiResult<bool>> CategoryAssign(int id, CategoryAssignRequest request)
        {
            var user = await _context.Products.FindAsync(id);
            if (user == null)
            {
                return new ApiErrorResult<bool>($"Sản phẩm với id {id} không tồn tại");
            }
            foreach (var category in request.Categories)
            {
                var productInCategory = await _context.ProductInCategories
                    .FirstOrDefaultAsync(x => x.CategoryId == int.Parse(category.Id)
                    && x.ProductId == id);
                if (productInCategory != null && category.Selected == false)
                {
                    _context.ProductInCategories.Remove(productInCategory);
                }
                else if (productInCategory == null && category.Selected)
                {
                    await _context.ProductInCategories.AddAsync(new ProductInCategory()
                    {
                        CategoryId = int.Parse(category.Id),
                        ProductId = id
                    });
                }
            }
            await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool>();
        }

        public async Task<List<ProductFeatureVm>> GetFeaturedProducts(int take)
        {
            //1. Select join
            var query = from p in _context.Products
                        where p.IsFeatured == true
                        from i in _context.ProductImages
                        where (i.ProductId == p.Id && i.IsDefault == true)
                        select new { p, i };

            var data = await query.Take(take).
                Select(x => new ProductFeatureVm()
                {
                    Id = x.p.Id,
                    Name = x.p.Name,
                    Details = x.p.Details,
                    Description = x.p.Description,
                    ImagePath = x.i.ImagePath,
                    Customer = x.p.Customer,
                    Localtion = x.p.Localtion,
                    Area = x.p.Area,
                    IsFeatured = x.p.IsFeatured
                }).ToListAsync();

            return data;
        }
        public async Task<int> AddListImages(ImagesCreateVm request)
        {
            if (request.ImageFiles.Count > 0)
            {
                for (int i = 0; i < request.ImageFiles.Count; i++)
                {
                    if (request.ImageFiles[i] != null)
                    {
                        var resultImage = await _photoService.AddPhotoAsync(request.ImageFiles[i]);
                        ProductImage image = new ProductImage();
                        image.ImagePath = resultImage.SecureUrl.AbsoluteUri;
                        image.PublicId = resultImage.PublicId;
                        image.FileSize = request.ImageFiles[i].Length;
                        image.ProductId = request.ProductId;
                        image.DateCreated = DateTime.Now;
                        image.IsDefault = false;
                        _context.ProductImages.Add(image);
                    }
                }
            }
            return await _context.SaveChangesAsync();
        }

        public async Task<int> SetMainImage(int imageId, int productId)
        {
            var images = await this.GetListImagesByProductId(productId);
            for (int i = 0; i < images.Count; i++)
            {
                if (images[i].Id != imageId)
                {
                    images[i].IsDefault = false;
                    _context.ProductImages.Update(images[i]);
                }
                else
                {
                    images[i].IsDefault = true;
                    _context.ProductImages.Update(images[i]);
                }
            }
            return await _context.SaveChangesAsync();
        }

        private async Task<List<ProductImage>> GetListImagesByProductId(int productId)
        {
            return await _context.ProductImages.Where(x => x.ProductId == productId).ToListAsync();
        }
    }
}