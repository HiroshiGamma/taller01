using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using api.src.data;
using api.src.models;
using Microsoft.EntityFrameworkCore;
using taller01.src.Dtos;
using taller01.src.Interfaces;

namespace taller01.src.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDBContext _context;
        public UserRepository(ApplicationDBContext context) 
        {
            _context = context;
        }

        public async Task<Product> AddProductToUser(int id, Product product)
        {
            var userModel = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
            if (userModel == null)
            {
                throw new Exception("User not found");
            }
            userModel.Products.Add(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task<User?> Delete(int id)
        {
            var userModel = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
            if (userModel == null)
            {
                throw new Exception("User not found");
            }
            _context.Remove(userModel);
            await _context.SaveChangesAsync();
            return userModel;
        }

        public async Task<List<User>> GetAll()
        {
            return await _context.Users.ToListAsync();
        }

        public IQueryable<User> GetAsQuery(int? age, string? gender, string? estado)
        {
            return _context.Users.AsQueryable();
        }

        public async Task<User?> GetById(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<User?> Post(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<User?> Put(int id, UpdateUserDto updateUserDto)
        {
            var userModel = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
            if (userModel == null)
            {
                throw new Exception("User not found");
            }
            userModel.Name = updateUserDto.Name;
            userModel.Birthdate = updateUserDto.Birthdate;
            userModel.GenderId = updateUserDto.GenderId;

            await _context.SaveChangesAsync();
            return userModel;
        }
        public async Task<User?> GetByRut(string rut) 
        {
            return await _context.Users.FindAsync(rut);
        }
    }
}