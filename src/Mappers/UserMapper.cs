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
                Nombre = userModel.Nombre,
                Rut = userModel.Rut,
                FechaNacimiento = userModel.FechaNacimiento,
                Correo = userModel.Correo, 
                GenderId = userModel.GenderId,
                EstadoId = userModel.EstadoId
              };
        }
    
    }
}