using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.src.data;
using api.src.Dtos;
using api.src.Helpers;
using api.src.models;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.EntityFrameworkCore;
using taller01.src.Dtos;
using taller01.src.Interface;

namespace taller01.src.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDBContext _context;
        private readonly Cloudinary _cloudinary;
        public ProductRepository(ApplicationDBContext context, Cloudinary cloudinary) 
        {
            _context = context;
            _cloudinary = cloudinary;
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

         public async Task<List<Product>> GetAll(QueryObject query)
        {
            var products = _context.Products.AsQueryable();

            if (!string.IsNullOrWhiteSpace(query.Name))
            {
                products = products.Where(x => x.Name.Contains(query.Name));
            }

            if (!string.IsNullOrWhiteSpace(query.SortBy))
            {
                if (query.SortBy.Equals("Name", StringComparison.OrdinalIgnoreCase))
                {
                    products = query.IsDescending ? products.OrderByDescending(x => x.Name) : products.OrderBy(x => x.Name);
                }   
            }

            var skipNumber = (query.PageNumber - 1) * query.PageSize;
        
            
            return await products.Skip(skipNumber).Take(query.PageSize).ToListAsync();
        }

        public async Task<Product?> GetById(int id)
        {
            return await _context.Products.FindAsync(id);
        }

        public async Task<Product> Post(Product product, IFormFile? image)
        {

            
            if (await ProductExistsAsync(product.Name, product.Type))
            {
                throw new Exception("A product with the same name and type already exists.");
            }
            
            if (image == null || image.Length == 0)
            {
                throw new Exception("Image is required");
            }
            
            if (image.ContentType != "image/jpeg" && image.ContentType != "image/png" &&
                !image.FileName.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase) &&
                !image.FileName.EndsWith(".png", StringComparison.OrdinalIgnoreCase))
                {
                    throw new Exception("Image must be a JPEG or PNG file");
                }
            
            if (image.Length > 10 * 1024 * 1024) 
            {
                throw new Exception("Image must be less than 10MB");
            }

            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(image.FileName, image.OpenReadStream()),
                Folder = "products_images" 
            };
            
            var uploadResult = await _cloudinary.UploadAsync(uploadParams);
         
            if (uploadResult.Error != null)
            {
                throw new Exception(uploadResult.Error.Message);
            }

            
            product.ImageUrl = uploadResult.SecureUrl.ToString();

            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();

            return product; 
        }

        private async Task<bool> ProductExistsAsync(string name, string type)
        {
            return await _context.Products.AnyAsync(p => p.Name == name && p.Type == type); 
        }

        public async Task<Product?> Put(int id, UpdateProductDto updateDto)
        {
      
            if(await ProductExistsAsync(updateDto.Name, updateDto.Type))
            {
                throw new Exception("A product with the same name and type already exists.");
            }

          
            var product = await _context.Products.FindAsync(id);
            if (product == null)
                return null;

            product.Name = updateDto.Name; 
            product.Price = updateDto.Price; 
            product.Stock = updateDto.Stock; 
            product.Type = updateDto.Type; 

          
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
            return product;
        }


        public async Task UpdateImage(Product product, IFormFile image)
        {
            if (!string.IsNullOrEmpty(product.ImageUrl))
            {
                var deletionParams = new DeletionParams(product.ImageUrl); 
                await _cloudinary.DestroyAsync(deletionParams);
            }

            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(image.FileName, image.OpenReadStream()), 
                Folder = "products_images"
            };

            var uploadResult = await _cloudinary.UploadAsync(uploadParams);

            if (uploadResult.Error != null)
            {
                throw new Exception(uploadResult.Error.Message);
            }

            product.ImageUrl = uploadResult.SecureUrl.ToString();
        }

        public async Task UpdateStock(int id, int quantity)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
                throw new InvalidOperationException("Producto no encontrado");

            product.Stock = quantity;
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
        }
    }
}