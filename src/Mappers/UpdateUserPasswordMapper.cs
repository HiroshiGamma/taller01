using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.src.models;
using taller01.src.Dtos;

namespace taller01.src.Mappers
{
    public static class UpdateUserPasswordMapper
    {
        public static UpdateUserPasswordDto ToUpdateUserPassword(this User userModel) 
        {
            return new UpdateUserPasswordDto 
            {
                Id = userModel.Id,
                Password = userModel.Password
            }; 
        }
    }
}