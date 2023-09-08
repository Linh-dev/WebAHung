using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eFashionShop.Data.Entities
{
    public class ProductImage
    {
        public int Id { get; set; }
        public string PublicId { get; set; }
        public int? ProductId { get; set; }
        public string ImagePath { get; set; }
        public string Caption { get; set; }
        public bool? IsDefault { get; set; }
        public bool? IsOutstanding { get; set; }
        public DateTime DateCreated { get; set; }
        public int? SortOrder { get; set; }
        public long? FileSize { get; set; }
        public bool IsFeatured { get; set; }
    }
}
