using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.src.Dtos;
using api.src.models;
using taller01.src.Dtos;

namespace taller01.src.Interfaces
{
    public interface IProductRepository
    {
        Task<List<Product>> GetAll();
        IQueryable<Product> GetAsQuery(string? searchText, string? type, string? order);
        Task<Product?> GetById(int id);
        Task<Product?> Post(Product product);
        Task<Product?> Put(int id, UpdateProductDto productDto);
        Task<Product?> Delete(int id);

    }
}