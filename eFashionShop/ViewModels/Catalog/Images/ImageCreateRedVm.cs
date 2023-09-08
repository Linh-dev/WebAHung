using Microsoft.AspNetCore.Http;

namespace eFashionShop.ViewModels.Catalog.Images
{
    public class ImageCreateRedVm
    {
        public string Caption { get; set; }
        public IFormFile File { get; set; }
        public bool IsFeatured { get; set; }
    }
}
