using eFashionShop.ViewModels.Common;
using eFashionShop.ViewModels.System.Users;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace eFashionShop.Application.Users
{
    public interface IUserService
    {
        Task<ApiResult<string>> Authencate(LoginRequest request);

        Task<ApiResult<bool>> Register(RegisterRequest request);

        Task<ApiResult<bool>> Update(int id, UserUpdateRequest request);

        Task<ApiResult<PagedResult<UserVm>>> GetUsersPaging(GetUserPagingRequest request);

        Task<ApiResult<UserVm>> GetById(int id);

        Task<ApiResult<bool>> Delete(int id);

        Task<ApiResult<bool>> RoleAssign(int id, RoleAssignRequest request);
    }
}