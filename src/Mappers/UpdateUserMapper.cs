using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.src.models;
using taller01.src.Dtos;

namespace taller01.src.Mappers
{
    public static class UpdateUserMapper
    {
        public static UpdateUserDto ToUpdateUserDto(this User userModel) 
        {
            return new UpdateUserDto
            {
                Nombre = userModel.Nombre,
                FechaNacimiento = userModel.FechaNacimiento,
                GenderId = userModel.GenderId
            };
        }
    }
}