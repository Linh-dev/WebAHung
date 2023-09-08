namespace eFashionShop.ViewModels.Catalog.Products
{
    public class ProductFeatureVm
    {
        public int Id { get; set; }
        public string Name { set; get; }
        public string Details { set; get; }
        public string Description { get; set; }
        public string ImagePath { get; set; }
        public string Customer { set; get; }
        public string Localtion { set; get; }
        public double Area { get; set; }
        public bool IsFeatured { get; set; }
    }
}
