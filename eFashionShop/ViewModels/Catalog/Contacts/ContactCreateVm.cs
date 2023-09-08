namespace eFashionShop.ViewModels.Catalog.Contacts
{
    public class ContactCreateVm
    {
        public string Name { set; get; }
        public string Email { set; get; }
        public string PhoneNumber { set; get; }
        public string Address { set; get; }
        public string Website { set; get; }
        public string Message { set; get; }
        public bool Default { get; set; }
    }
}
