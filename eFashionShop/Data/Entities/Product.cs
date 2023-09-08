using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eFashionShop.Data.Entities
{
    public class Product
    {
        public int Id { set; get; }
        public string Name { set; get; }
        public string Description { set; get; }
        public string Details { set; get; }
        public string Customer { set; get; }
        public string Localtion { set; get; }
        public double Area { get; set; }
        public DateTime DateCreated { set; get; }
        public bool IsFeatured { get; set; }
        public List<ProductInCategory> ProductInCategories { get; set; }
        public List<ProductImage> ProductImages { get; set; }
    }
}
