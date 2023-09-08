using eFashionShop.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eFashionShop.Data.Entities
{
    public class Category
    {
        public int Id { set; get; }
        public string Name { set; get; }
        public int? SortOrder { set; get; }
        public bool IsShowOnHome { set; get; }
        public int? ParentId { set; get; }
        public Status Status { set; get; }
        public string ImagePublishId { set; get; }
        public string ImageUrl { set; get; }
        public List<ProductInCategory> ProductInCategories { get; set; }
    }
}
