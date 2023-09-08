using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace eFashionShop.ViewModels.Catalog.ProductImages
{
    public class ImagesCreateVm
    {
        public int ProductId { get; set; }
        public List<IFormFile> ImageFiles { get; set; }
    }
}
