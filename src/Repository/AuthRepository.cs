using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using taller01.src.Dtos;
using taller01.src.Interface;
using taller01.src.models;

namespace taller01.src.Repository
{
    public class AuthRepository : IAuthRepository
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly SignInManager<AppUser> _signInManager;

        public AuthRepository(UserManager<AppUser> userManager, ITokenService tokenService, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _signInManager = signInManager;
        }
        public async Task<NewUserDto> RegisterAsync(RegisterDto registerDto)
        {
            if (string.IsNullOrEmpty(registerDto.Password))
            {
                throw new ArgumentException("Password is required");
            }

            var user = new AppUser
            {
                Rut = registerDto.Rut,
                UserName = registerDto.Username,
                DateOfBirth = registerDto.Birthdate, 
                Email = registerDto.Email,
                Gender = registerDto.Gender,
                Enabled = registerDto.Enabled
            };

            var createUser = await _userManager.CreateAsync(user, registerDto.Password);

            if (!createUser.Succeeded)
            {
                throw new InvalidOperationException(string.Join("; ", createUser.Errors.Select(e => e.Description)));
            }

            var roleResult = await _userManager.AddToRoleAsync(user, "User");

            if (!roleResult.Succeeded)
            {
                throw new InvalidOperationException(string.Join("; ", roleResult.Errors.Select(e => e.Description)));
            }

            var roles = await _userManager.GetRolesAsync(user);

            // El usuario es creado exitosamente
            return new NewUserDto
            {
                Username = user.UserName,
                Email = user.Email,
                Role = roles.First(),
                Token = await _tokenService.CreateToken(user)
            };
        }
        public async Task<NewUserDto> LoginAsync(LoginDto loginDto)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Email == loginDto.Email);

            if (user == null)
            {
                throw new UnauthorizedAccessException("Invalid user or password");
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

            if (!result.Succeeded)
            {
                throw new UnauthorizedAccessException("Invalid user or password");
            }

            if (!user.Enabled)
            {
                throw new Exception("La cuenta está deshabilitada. Contacte al administrador.");
            }

            var roles = await _userManager.GetRolesAsync(user);

            return new NewUserDto
            {
                Username = user.UserName!,
                Email = user.Email!,
                Role = roles.First(),
                Token = await _tokenService.CreateToken(user)
            };
        }
    }
}