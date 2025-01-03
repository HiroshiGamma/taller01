using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.src.Dtos;
using api.src.Helpers;
using api.src.models;
using taller01.src.Dtos;

namespace taller01.src.Interface
{
    public interface IProductRepository
    {
        Task<List<Product>> GetAll(QueryObject query);
        Task<Product?> GetById(int id);

        Task<Product> Post(Product product, IFormFile? image);
        Task<Product?> Put(int id, UpdateProductDto productDto);
        Task<Product?> Delete(int id);
        Task UpdateImage(Product product, IFormFile image);
        Task UpdateStock(int id, int quantity);
        
    }
}