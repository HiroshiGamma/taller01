using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using taller01.src.models;
using taller01.src.Dtos;

namespace api.src.Mappers
{
    public static class UserMapper
    {
        public static AppUser ToAppUserFromLoginDto(this LoginDto loginDto)
        {
            return new AppUser
            {
                UserName = loginDto.UserName,

            };
        }

        public static AppUser ToAppUserFromNewUserDto(this NewUserDto newUserDto)
        {
            return new AppUser
            {
                UserName = newUserDto.Username,
                Email = newUserDto.Email,

            };
        }

        public static AppUser ToAppUserFromRegisterDto(this RegisterDto registerDto)
        {
            return new AppUser
            {
                UserName = registerDto.Username,
                Rut = registerDto.Rut,
                DateOfBirth = registerDto.Birthdate,
                Email = registerDto.Email,
            };
        }
    }
}