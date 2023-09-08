using eFashionShop.Data.EF;
using eFashionShop.Data.Entities;
using eFashionShop.Exceptions;
using eFashionShop.ViewModels.Catalog.Contacts;
using eFashionShop.ViewModels.WebAppViewModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace eFashionShop.Application.Contacts
{
    public class ContactService : IContactService
    {
        private readonly EShopDbContext _context;

        public ContactService(EShopDbContext context)
        {
            _context = context;
        }
        public async Task<bool> Create(ContactCreateVm res)
        {

            if (res == null) throw new EShopException("Create contact fail!");
            var contact = new Contact
            {
                Name = res.Name,
                Email = res.Email,
                PhoneNumber = res.PhoneNumber,
                Address = res.Address,
                Website = res.Website,
                Message = res.Message,
                Default = res.Default,
                Status = Data.Enums.Status.InActive,
            };
            _context.Contacts.Add(contact);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<Contact> Default()
        {
            var data = from c in _context.Contacts where c.Default == true select c;
            return await data.FirstOrDefaultAsync();
        }

        public async Task<bool> Delete(int id)
        {
            if (id < 0) throw new EShopException("Delete contact fail!");
            var contact = await _context.Contacts.FindAsync(id);
            _context.Contacts.Remove(contact);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<List<Contact>> GetAll()
        {
            var res = from c in _context.Contacts select c;
            return await res.ToListAsync();
        }

        public async Task<bool> SendMail(CustomerContactModel res)
        {
            try
            {
                var From = "baclinh0123@gmail.com";
                var To = "hungcao38@gmail.com";
                var Subject = "Thông tin KH muốn liên hệ";
                var Body = $"<h2>Tên: {res.Name}</h2><br /><h2>Email: {res.Email}</h2><br /><h2>Phone: {res.Phone}</h2><br /><h2>Lời Nhắn: {res.Content}</h2>";
                var smtpClient = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    Credentials = new NetworkCredential(From, "vsvdrlxasxapvfik"),
                    EnableSsl = true,
                };
                var mailMessage = new MailMessage
                {
                    From = new MailAddress(From),
                    Subject = Subject,
                    Body = Body,
                    IsBodyHtml = true,
                };
                mailMessage.To.Add(To);

                smtpClient.Send(mailMessage);
                return true;

            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public Task<bool> SetDefault(int id)
        {
            throw new System.NotImplementedException();
        }

        public async Task<bool> Update(Contact res)
        {
            if (res == null) throw new EShopException("Update fail!");
            _context.Contacts.Update(res);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
