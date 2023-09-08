using eFashionShop.Data.Enums;
using Microsoft.AspNetCore.Http;

namespace eFashionShop.ViewModels.Catalog.Categories
{
    public class CategoryCreateVm
    {
        public string Name { set; get; }
        public bool IsShowOnHome { set; get; }
        public int? ParentId { set; get; }
        public int? ImageId { get; set; }
        public IFormFile File { get; set; }
        public Status Status { set; get; }
    }
}
