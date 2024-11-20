using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.src.data;
using api.src.models;
using Microsoft.EntityFrameworkCore;
using taller01.src.Dtos;
using taller01.src.Interfaces;

namespace taller01.src.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDBContext _context;
        public ProductRepository(ApplicationDBContext context) 
        {
            _context = context;
        }
        public async Task<Product?> Delete(int id)
        {
            var productModel = await _context.Products.FirstOrDefaultAsync(x => x.Id == id);
            if (productModel == null)
            {
                throw new Exception("Product not found");
            }
            _context.Remove(productModel);
            await _context.SaveChangesAsync();
            return productModel;
        }

        public async Task<List<Product>> GetAll()
        {
            return await _context.Products.ToListAsync();
        }

        public IQueryable<Product> GetAsQuery(string? searchText, string? type, string? order)
        {
            return _context.Products.AsQueryable();
        }

        public async Task<Product?> GetById(int id)
        {
            return await _context.Products.FindAsync(id);
        }
        

        public async Task<Product?> Post(Product product)
        {
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task<Product?> Put(int id, ProductDto productDto)
        {
            var productModel = await _context.Products.FirstOrDefaultAsync(x => x.Id == id);
            if (productModel == null)
            {
                throw new Exception("Product not found");
            }
            productModel.Nombre = productDto.Nombre;
            productModel.Tipo = productDto.Tipo;
            productModel.Precio = productDto.Precio;
            productModel.Stock = productDto.Stock;
            
            await _context.SaveChangesAsync();
            return productModel;
        }

        
    }
}