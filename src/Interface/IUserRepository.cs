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
        Task<User?> GetById(int id);
        IQueryable<User> GetAsQuery(int? age, string? gender, string? estado);
        Task<User?> Post(User user);
        Task<User?> Put(int id, UpdateUserDto updateUserDto);
        Task<User?> Delete(int id);   
        Task<Product> AddProductToUser(int id, Product product);
        Task<User?> GetByRut(string rut);
    }
}