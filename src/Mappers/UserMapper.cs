using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.src.models;
using taller01.src.Dtos;

namespace taller01.src.Mappers
{
    public static class UserMapper
    {
        public static UserDto ToUserDto(this User userModel) 
        {
            return new UserDto 
            {
                Id = userModel.Id,
                Name = userModel.Name,
                Rut = userModel.Rut,
                Birthdate = userModel.Birthdate,
                Mail = userModel.Mail, 
                Password = userModel.Password,
                GenderId = userModel.GenderId,
                StatusId = userModel.StatusId
              };
        }
        public static User ToUser(this UserDto userDto) 
        {
            return new User
            {
                Id = userDto.Id,
                Name = userDto.Name,
                Rut = userDto.Rut,
                Birthdate = userDto.Birthdate,
                Mail = userDto.Mail, 
                Password = userDto.Password,
                GenderId = userDto.GenderId,
                StatusId = userDto.StatusId, 
                RoleId = 2  
            };
        }
    }
}