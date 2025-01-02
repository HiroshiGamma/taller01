using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.src.Repository;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using taller01.src.Dtos;
using taller01.src.models;

namespace taller01.src.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin,User")]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<AppUser> _signInManager;

        private readonly UserRepository _userRepository;

        public AccountController(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, SignInManager<AppUser> signInManager, UserRepository userRepository)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _userRepository = userRepository;
        }

        [HttpPut("UpdatePassword")]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> UpdatePassword([FromRoute] string id, [FromBody] string oldPassword, string newPassword) 
        {
            var user = await _userManager.FindByIdAsync(id); 
            if (user == null)
            {
                throw new Exception("Usuario no encontrado");
            }
            var changePasswordResult = await _userManager.ChangePasswordAsync(user, oldPassword, newPassword);
            if (!changePasswordResult.Succeeded)
            {
                return BadRequest(ModelState);
            }
            return Ok();
        }
        
       /// <summary>
        /// Metodo para habilitar o deshabilitar un usuario
        /// </summary>
        /// <param name="rut"> rut del usuario a habilitar o deshabilitar </param>
        /// <param name="enable"> variable booleana encargada de cambiar el estado </param>
        /// <returns> Ok, si el usuario fue cambiado con exito, Error, si no esta autorizado.</returns>
        [HttpPut("enable-disable/{rut}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> EnableDisableUser([FromRoute] string rut, [FromBody] bool enable)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Aquí validamos si el rol del usuario autenticado es Admin
            var currentUser = await _userManager.GetUserAsync(User);
            if (!await _userManager.IsInRoleAsync(currentUser!, "Admin"))
            {
                return Unauthorized();
            }

            await _userRepository.EnableDisableUser(rut, enable);
            return Ok();
        }

        [HttpDelete]
        [Route("{id}")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> DeleteAccount([FromRoute] string id)
        {
            // Get the current logged-in user
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound("User not found");
            }

            // Attempt to delete the user
            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                // Log the user out after account deletion
                await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);

                return RedirectToAction("AccountDeleted", "Home"); // Redirect to a confirmation page
            }

            // Handle errors
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            
            return BadRequest("Error"); // Show an error page
        }
        [HttpPost("logout")]
        [Authorize(Roles = "Admin,User")]

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok(new { message = "Logged out successfully" });
        }

    }
}