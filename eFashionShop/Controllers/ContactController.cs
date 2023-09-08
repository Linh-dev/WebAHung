using eFashionShop.Application.Contacts;
using eFashionShop.Data.Entities;
using eFashionShop.ViewModels.WebAppViewModel;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace eFashionShop.Controllers
{
    public class ContactController : Controller
    {
        private readonly IContactService _contactService;
        public ContactController(IContactService contactService)
        {
            _contactService = contactService;
        }
        public async Task<IActionResult> Index()
        {
            var data = await _contactService.Default();
            var res = data;
            if(res == null)
            {
                res = new Contact()
                {
                    Id = -1,
                    Name = "Null",
                    Email = "Null",
                    PhoneNumber = "Null",
                    Address = "Null",
                    Website = "Null",
                    Message = "Null",
                    Default = false,
                    Status = Data.Enums.Status.Active
                };
            }
            return View(res);
        }
        [HttpPost]
        public async Task<IActionResult> SendInformation(string name, string email, string phone, string content)
        {
            var contact = new CustomerContactModel
            {
                Name = name,
                Email = email,
                Phone = phone,
                Content = content
            };
            var isCheck = await _contactService.SendMail(contact);
            if (isCheck) ViewBag.Message = "Cảm ơn bạn đã liên hệ!";
            var data = await _contactService.Default();
            var res = data;
            if (res == null)
            {
                res = new Contact()
                {
                    Id = -1,
                    Name = "Null",
                    Email = "Null",
                    PhoneNumber = "Null",
                    Address = "Null",
                    Website = "Null",
                    Message = "Null",
                    Default = false,
                    Status = Data.Enums.Status.Active
                };
            }
            return View("Index", res);
        }
    }
}
