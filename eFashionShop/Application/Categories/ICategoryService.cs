using eFashionShop.ViewModels.Catalog.Categories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eFashionShop.Application.Categories
{
    public interface ICategoryService
    {
        Task<List<CategoryVm>> GetAll();

        Task<CategoryVm> GetById(int id);
        Task<CategoryUpdateVm> GetByIdForUpdate(int id);
        Task<bool> Create(CategoryCreateVm categoryVm);
        Task<bool> Delete(int id);
        Task<List<CategoryVm>> GetListParent();
        Task<bool> Edit(CategoryUpdateVm categoryUpdateVm);
    }
}
