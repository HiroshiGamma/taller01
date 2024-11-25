using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AccountController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
        }

        [HttpPut]
        [Route("{id}")]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> UpdateUser([FromRoute] string id, [FromBody] UpdateUserDto updateUserDto) {
            AppUser? user = await _userManager.FindByIdAsync(id) as AppUser; 
            if (user == null)
            {
                throw new Exception("Usuario no encontrado");
            }
            if (updateUserDto.Name != null) 
            {
                user.UserName = updateUserDto.Name;
            }
            if (updateUserDto.Birthdate != null) 
            {
                user.DateOfBirth = updateUserDto.Birthdate;
            }
            if (updateUserDto.Gender != null) 
            {
                user.Gender = updateUserDto.Gender;
            }
            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                return Ok();
            }
            return BadRequest();
        }
        [HttpPut]
        [Route("{id}")]
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

        [HttpPut]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EnableDisableUser([FromBody] string id) 
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                throw new Exception("Usuario no encontrado");
            }
            var currentRole = await _userManager.GetRolesAsync(user);

            foreach(var role in currentRole)
            {
                if (role == "User") 
                {
                    var removeResult = await _userManager.RemoveFromRolesAsync(user, currentRole);
                    if (!removeResult.Succeeded)
                    {
                        return BadRequest("Failed to remove current roles");
                    }
                    var addResult = await _userManager.AddToRoleAsync(user, "Disabled");
                    if (!addResult.Succeeded)
                    {
                        return BadRequest("Failed to add new role");
                    }

                    return Ok("Successfully changed user's role to disabled");
                }
                if (role == "Disabled") 
                {
                    var removeResult = await _userManager.RemoveFromRolesAsync(user, currentRole);
                    if (!removeResult.Succeeded)
                    {
                        return BadRequest("Failed to remove current roles");
                    }
                    var addResult = await _userManager.AddToRoleAsync(user, "User");
                    if (!addResult.Succeeded)
                    {
                        return BadRequest("Failed to add new role");
                    }

                    return Ok("Successfully changed user's role to user");
                }

            }
            return BadRequest();
            
        }
        [HttpPost]
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