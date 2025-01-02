using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using api.src.Helpers;
using taller01.src.Dtos;
using taller01.src.models;

namespace api.src.Interface
{
    public interface IUserRepository
    {
        Task<AppUser?> UpdateUser(UpdateUserDto userDto, ClaimsPrincipal currentUser);
        Task<List<AppUser>> GetUsers(QueryObject query);
        Task EnableDisableUser(string rut, bool enable);
        Task UpdatePassword(ClaimsPrincipal currentUser, string newPassword, string oldPassword, string newPasswordConfirmation);
        Task DeleteUser(ClaimsPrincipal currentUser);

    }
}