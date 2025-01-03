using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using taller01.src.models;
using taller01.src.Dtos;
using api.src.Dtos.User;

namespace api.src.Mappers
{
    public static class UserMapper
    {
        public static UserDto ToUserDtoFromUser(this AppUser user)
        {
            return new UserDto
            {
                Id = user.Id,
                Rut = user.Rut,
                Name = user.UserName!,
                Birthdate = user.DateOfBirth,
                Email = user.Email!,
                Gender = user.Gender,
                Enabled = user.Enabled
            };
        }
    }
}