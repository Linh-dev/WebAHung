using System;
using System.Collections.Generic;
using System.Text;

namespace eFashionShop.ViewModels.Catalog.Categories
{
    public class CategoryVm
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? ParentId { get; set; }
        public string ImagePublishId { get; set; }
        public string ImageUrl { get; set; }
    }
}