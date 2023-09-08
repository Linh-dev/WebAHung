using eFashionShop.Data.Entities;
using eFashionShop.ViewModels.Catalog.Contacts;
using eFashionShop.ViewModels.WebAppViewModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eFashionShop.Application.Contacts
{
    public interface IContactService
    {
        Task<bool> Update(Contact res);
        Task<List<Contact>> GetAll();
        Task<Contact> Default();
        Task<bool> SetDefault(int id);
        Task<bool> Delete(int id);
        Task<bool> Create(ContactCreateVm res);
        Task<bool> SendMail(CustomerContactModel res);
    }
}
