using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using taller01.src.Dtos;

namespace taller01.src.Interface
{
    public interface IAuthRepository
    {
        Task<NewUserDto> RegisterAsync(RegisterDto registerDto);
        Task<NewUserDto> LoginAsync(LoginDto loginDto);
    }
}