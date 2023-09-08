using eFashionShop.ViewModels.Catalog.Images;
using eFashionShop.ViewModels.Catalog.ProductImages;
using eFashionShop.ViewModels.Catalog.Products;
using eFashionShop.ViewModels.Common;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eFashionShop.Application.Images
{
    public interface IImageService
    {
        Task<PagedResult<ImageVm>> GetAll(GetManageImagePagingRequest request);
        Task<List<ImageVm>> GetFeaturedImages(int count);
        Task<bool> SetFeaturedImage(int id);
        Task<bool> AddImage(List<ImageCreateRedVm> red);
        Task<bool> DeleteImage(int id);
    }
}
