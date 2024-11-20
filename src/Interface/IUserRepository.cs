using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.src.models;
using taller01.src.Dtos;

namespace taller01.src.Interfaces
{
    public interface IUserRepository
    {
        Task<List<User>> GetAll();
        Task<User?> GetById();
        Task<User?> Post(User user);
        Task<User?> Put(int id, UserDto userDto);
        Task<User?> Delete(int id);   
    }
}