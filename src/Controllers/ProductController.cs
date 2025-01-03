using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.src.data;
using api.src.Dtos;
using api.src.Helpers;
using api.src.Mappers;
using api.src.models;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using taller01.src.Dtos;
using taller01.src.Dtos.Product;
using taller01.src.Interface;
using taller01.src.Mappers;
using taller01.src.Repository;

namespace taller01.src.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        public ProductController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }
    
        // Retrieves a list of products based on the provided filters and order.
        // Parameters:
        // - productDto: DTO containing product filters (Name, Type).
        // - order: Optional order parameter to sort products by price (asc or desc).
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] QueryObject query)
        {
            if (!ModelState.IsValid) 
            {
                return BadRequest(ModelState); 
            }

            var products = await _productRepository.GetAll(query); 
            var productDto = products.Select(p => p.ToProductDto()); 
            return Ok(new 
            {
                Total = products.Count(), 
                Data = productDto 
            });
        }

        // Retrieves a product by its ID.
        // Parameters:
        // - id: The ID of the product to retrieve.
        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var product = await _productRepository.GetById(id);

            if (product == null)
            {
                return NotFound();
            }
            return Ok(product.ToProductDto());
        }

        // Creates a new product.
        // Parameters:
        // - createProductDto: DTO containing product details and image file.
        [HttpPost]
        [Consumes("multipart/form-data")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> PostAsync([FromForm] CreateProductWithImageDto createProductDto)
        {
            var productModel = new Product
            {
                Name = createProductDto.Name,
                Price = createProductDto.Price,
                Stock = createProductDto.Stock,
                Type = createProductDto.Type
            };
            await _productRepository.Post(productModel, createProductDto.Image); 

            return CreatedAtAction(nameof(GetById), new { id = productModel.Id }, productModel.ToProductDto());
        }

        // Updates an existing product by its ID.
        // Parameters:
        // - id: The ID of the product to update.
        // - updateProductDto: DTO containing updated product details.
        [HttpPut]
        [Route("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Put([FromRoute] int id, [FromBody] UpdateProductDto updateProductDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existingProduct = await _productRepository.GetById(id);
            if (existingProduct == null)
                return NotFound("Product not found");

            existingProduct.Name = updateProductDto.Name;
            existingProduct.Price = updateProductDto.Price;
            existingProduct.Stock = updateProductDto.Stock;
            existingProduct.Type = updateProductDto.Type;

            if (updateProductDto.Image != null)
            {
                await _productRepository.UpdateImage(existingProduct, updateProductDto.Image); 
            }
            var updatedProduct = await _productRepository.Put(id, updateProductDto);
            
            if (updatedProduct == null)
                return NotFound("Product not found"); 

            return Ok(updatedProduct.ToProductDto());
        }

        // Deletes a product by its ID.
        // Parameters:
        // - id: The ID of the product to delete.
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _productRepository.Delete(id);
            if (product == null)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}