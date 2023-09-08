using eFashionShop.ViewModels.Catalog.ProductImages;
using eFashionShop.ViewModels.Catalog.Products;
using eFashionShop.ViewModels.Common;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eFashionShop.Application.Products
{
    public interface IProductService
    {
        Task<int> Create(ProductCreateRequest request);

        Task<int> Update(ProductUpdateRequest request);

        Task<int> Delete(int productId);

        Task<ProductVm> GetById(int productId);

        Task<PagedResult<ProductVm>> GetAllPaging(GetManageProductPagingRequest request);
        Task<List<ProductVm>> GetAll();
        Task<List<ProductVm>> GetAllByCategoriesId(int categoriesId);
        Task<int> AddImage(int productId, ProductImageCreateRequest request);
        Task<int> AddListImages(ImagesCreateVm request);
        Task<int> SetMainImage(int imageId, int productId);

        Task<int> RemoveImage(int imageId);

        Task<int> UpdateImage(int imageId, ProductImageUpdateRequest request);

        Task<ProductImageViewModel> GetImageById(int imageId);

        Task<List<ProductImageViewModel>> GetListImages(int productId);

        Task<ApiResult<bool>> CategoryAssign(int id, CategoryAssignRequest request);

        Task<List<ProductFeatureVm>> GetFeaturedProducts(int take);
    }
}