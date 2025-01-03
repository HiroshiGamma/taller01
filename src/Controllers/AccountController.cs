using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.src.Helpers;
using api.src.Interface;
using api.src.Mappers;
using api.src.Repository;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using taller01.src.Dtos;
using taller01.src.Dtos.User;
using taller01.src.models;

namespace taller01.src.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        

        private readonly IUserRepository _userRepository;

        public AccountController(UserManager<AppUser> userManager, IUserRepository userRepository)
        {
            _userManager = userManager;
            _userRepository = userRepository;
        }
        [HttpGet("users")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetUsers([FromQuery] QueryObject query)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var users = await _userRepository.GetUsers(query);
            var usersDto = users.Select(user => user.ToUserDtoFromUser()).ToList();

            return Ok(new
            {
                Total = users.Count(),
                Users = usersDto
            });
        }
        [HttpPut("update-user")]
        [Authorize]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateUserDto userDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var user = await _userRepository.UpdateUser(userDto, User);
                if (user == null)
                {
                    return NotFound();
                }

                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("update-password")]
        [Authorize]
        public async Task<IActionResult> UpdatePassword([FromBody] UpdateUserPasswordDto updatePasswordDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _userRepository.UpdatePassword(User, updatePasswordDto.NewPassword, updatePasswordDto.Password, updatePasswordDto.ConfirmPassword);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        
       /// <summary>
        /// Metodo para habilitar o deshabilitar un usuario
        /// </summary>
        /// <param name="rut"> rut del usuario a habilitar o deshabilitar </param>
        /// <param name="enable"> variable booleana encargada de cambiar el estado </param>
        /// <returns> Ok, si el usuario fue cambiado con exito, Error, si no esta autorizado.</returns>
        [HttpPut("enable-disable/{rut}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EnableDisableUser([FromRoute] string rut, [FromBody] bool enable)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var currentUser = await _userManager.GetUserAsync(User);
            if (!await _userManager.IsInRoleAsync(currentUser!, "Admin"))
            {
                return Unauthorized();
            }

            await _userRepository.EnableDisableUser(rut, enable);
            return Ok();
        }

        [HttpDelete("delete-user")]
        [Authorize]
        public async Task<IActionResult> DeleteUser()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _userRepository.DeleteUser(User);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

    }
}