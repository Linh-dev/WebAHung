using eFashionShop.ViewModels.System.Roles;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace eFashionShop.Application.Roles
{
    public interface IRoleService
    {
        Task<List<RoleVm>> GetAll();
    }
}