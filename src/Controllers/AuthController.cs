using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using taller01.src.Dtos;
using taller01.src.Interface;
using taller01.src.models;

namespace taller01.src.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly SignInManager<AppUser> _signInManager;
        public AuthController(UserManager<AppUser> userManager, ITokenService tokenService, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _signInManager = signInManager;
        }

        // This method registers a new user.
        // Parameters:
        // - registerDto: Contains the username, email, and password for the new user.
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            try {
                if(!ModelState.IsValid) {
                    return BadRequest(ModelState);
                }

                var appUser = new AppUser
                {
                    UserName = registerDto.Username,
                    Email = registerDto.Email,
                };

                if (string.IsNullOrEmpty(registerDto.Password))
                {
                    return BadRequest("Password cannot be null or empty.");
                }

                var createUser = await _userManager.CreateAsync(appUser, registerDto.Password);

                if(createUser.Succeeded) {
                    var roleResult = await _userManager.AddToRoleAsync(appUser, "User");
                    if(roleResult.Succeeded) {
                        return Ok(
                            new NewUserDto 
                            {
                                Username = appUser.UserName,
                                Email = appUser.Email,
                                Token = _tokenService.CreateToken(appUser)
                            }
                        );
                    } else {
                        return StatusCode(500, roleResult.Errors);
                    }
                }
                else 
                {
                    return StatusCode(500, createUser.Errors);
                }

            } catch (Exception ex) {
                return StatusCode(500, ex.Message);
            }
        }

        
        // This method logs in an existing user.
        // Parameters:
        // - loginDto: Contains the username and password for the user.
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            try {
                if(!ModelState.IsValid) {
                    return BadRequest(ModelState);
                }

                var user = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == loginDto.UserName);
                if(user == null) return Unauthorized("Invalid username or password.");


                var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
                if(!result.Succeeded) return Unauthorized("Invalid username or password.");

                return Ok(
                    new NewUserDto
                    {
                        Username = user.UserName!,
                        Email = user.Email!,
                        Token = _tokenService.CreateToken(user)
                    }
                );
            }catch (Exception ex) {
                return StatusCode(500, ex.Message);
            }
        }
    }
}