using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using api.src.Dtos.User;
using api.src.Interface;
using api.src.Helpers;
using api.src.models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using api.src.data;
using taller01.src.models;
using taller01.src.Dtos;


namespace api.src.Repository
{
        
    /// <summary>
    /// Repositorio de usuario
    /// </summary>
    public class UserRepository : iUserRepository
    {

        private readonly ApplicationDBContext _dataContext;


        private readonly UserManager<AppUser> _userManager;
        


        public UserRepository(ApplicationDBContext dataContext, UserManager<AppUser> userManager)
        {
            _dataContext = dataContext;
            _userManager = userManager;

        }


        public async Task<List<AppUser>> GetUsers(QueryObject query)
        {
            var users = _dataContext.Users.AsQueryable();

            if (!string.IsNullOrWhiteSpace(query.Name))
            {
                users = users.Where(x => x.UserName!.Contains(query.Name));
            }

            if (!string.IsNullOrWhiteSpace(query.SortBy))
            {
                if (query.SortBy.Equals("Name", StringComparison.OrdinalIgnoreCase))
                {
                    users = query.IsDescending ? users.OrderByDescending(x => x.UserName) : users.OrderBy(x => x.UserName);
                }
            }

            var skipNumber = (query.PageNumber - 1) * query.PageSize;

            return await users.Skip(skipNumber).Take(query.PageSize).ToListAsync();
        }

        /// <summary>
        /// Meotodo para habilitar o deshabilitar un usuario
        /// </summary>
        /// <param name="rut"> rut del usuario a cambiar su estado </param>
        /// <param name="enable"> variable booleana para cambiar </param>
        public async Task EnableDisableUser(string rut, bool enable)
        {

            
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Rut == rut);
            if (user == null)
            {
                throw new Exception("User not found");
            }

            user.Enabled = enable; // Si estás utilizando un campo "Enabled" personalizado, mantenlo.

            // Si el campo Enabled es parte de Identity, necesitarías hacer un poco más con UserManager.
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                throw new Exception("Error enabling or disabling the user");
            }

            await _dataContext.SaveChangesAsync();
        }


        /// <summary>
        /// Metodo para actualizar los datos de un usuario
        /// </summary>
        /// <param name="userDto"> Dto, con datos del usuario actualizados </param>
        /// <param name="currentUser">Usuario actual en el sistema</param>
        /// <returns>Usuario actualizado</returns>
        public async Task<AppUser?> UpdateUser(UpdateUserDto userDto, ClaimsPrincipal currentUser)
        {
            var user = await _userManager.GetUserAsync(currentUser);
            if (user == null)
            {
                throw new Exception("User not found");
            }

            // Actualizar la información del perfil
            user.UserName = userDto.Name;
            user.DateOfBirth = userDto.DateOfBirth;
            user.Gender = userDto.Gender;

            // Actualizar en Identity
            var result = await _userManager.UpdateAsync(user);
            
            if (!result.Succeeded)
            {
                throw new Exception("Error updating user profile");
            }

            await _dataContext.SaveChangesAsync();
            return user;
        }

        /// <summary>
        /// Metodo para actualizar la contraseña de un usuario
        /// </summary>
        /// <param name="currentUser"> Usuario actual del sistema</param>
        /// <param name="newPassword"> Nueva contraseña del usuario </param>
        /// <param name="oldPassword"> Contraseña antigua </param>
        /// <param name="newPasswordConfirmation"> Confirmación de la nueva contraseña </param>
        public async Task UpdatePassword(ClaimsPrincipal currentUser, string newPassword, string oldPassword, string newPasswordConfirmation)
        {
            var user = await _userManager.GetUserAsync(currentUser);
            if (user == null)
            {
                throw new Exception("User not found");
            }

            // Verificar la contraseña actual
            var result = await _userManager.CheckPasswordAsync(user, oldPassword);
            if (!result)
            {
                throw new Exception("Invalid password");
            }

            // Validar que las contraseñas coinciden
            if (newPassword != newPasswordConfirmation)
            {
                throw new Exception("Passwords do not match");
            }

            // Cambiar la contraseña
        
            var changePasswordResult = await _userManager.ChangePasswordAsync(user, oldPassword, newPassword);
            
            if (!changePasswordResult.Succeeded)
            {
                throw new Exception("Error changing password");
            }
            await _dataContext.SaveChangesAsync();
        }

        /// <summary>
        /// Metodo para eliminar su cuenta
        /// </summary>
        /// <param name="currentUser"> Usuario actual del sistema </param>
        public async Task DeleteUser(ClaimsPrincipal currentUser)
        {
            var user = await _userManager.GetUserAsync(currentUser);
            if (user == null)
            {
                throw new Exception("User not found");
            }

            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                throw new Exception("Error deleting user");
            }

            await _dataContext.SaveChangesAsync();
    }
    
    }
}