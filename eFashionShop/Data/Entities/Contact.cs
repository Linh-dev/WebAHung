using eFashionShop.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eFashionShop.Data.Entities
{
    public class Contact
    {
        public int Id { set; get; }
        public string Name { set; get; }
        public string Email { set; get; }
        public string PhoneNumber { set; get; }
        public string Hotline { set; get; }
        public string Address { set; get; }
        public string Website { set; get; }
        public string Message { set; get; }
        public bool Default { get; set; }
        public Status Status { set; get; }
    }
}
