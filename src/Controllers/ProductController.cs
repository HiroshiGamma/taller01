using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.src.data;
using api.src.Dtos;
using api.src.Mappers;
using api.src.models;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using taller01.src.Dtos;
using taller01.src.Interfaces;
using taller01.src.Mappers;
using taller01.src.Repository;

namespace taller01.src.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        private readonly Cloudinary _cloudinary;
        public ProductController(IProductRepository productRepository,  Cloudinary cloudinary)
        {
            _productRepository = productRepository;
            _cloudinary = cloudinary;
        }
    
        // Retrieves a list of products based on the provided filters and order.
        // Parameters:
        // - productDto: DTO containing product filters (Name, Type).
        // - order: Optional order parameter to sort products by price (asc or desc).
        [HttpGet]
        public IActionResult Get([FromQuery] ProductDto productDto, string? order)
        {
            var productsQuery = _productRepository.GetAsQuery(productDto.Name, productDto.Type, order);

            if (!string.IsNullOrEmpty(productDto.Name))
            {
                productsQuery = productsQuery.Where(p => p.Name.Contains(productDto.Name));
            }

            if (!string.IsNullOrEmpty(productDto.Type))
            {
                productsQuery = productsQuery.Where(p => p.Type == productDto.Type);
            }

            if (order == "asc")
            {
                productsQuery = productsQuery.OrderBy(p => p.Price);
            }
            else if (order == "desc")
            {
                productsQuery = productsQuery.OrderByDescending(p => p.Price);
            }

            var products = productsQuery
                .Where(p => p.Stock > 0)
                .ToList()
                .Select(p => p.ToProductDto());

            return Ok(products);
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
            return Ok(product);
        }

        // Creates a new product.
        // Parameters:
        // - createProductDto: DTO containing product details and image file.
        [HttpPost]
        [Consumes("multipart/form-data")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> postAsync([FromForm] CreateProductDto createProductDto)
        {
            if (createProductDto.Image == null || createProductDto.Image.Length == 0) 
            {
                return BadRequest("Image is required");
            }
            if (createProductDto.Image.ContentType != "image/png" && createProductDto.Image.ContentType != "image/jpeg")
            {
                return BadRequest("Only PNG and JPG images are allowed.");
            }
            if (createProductDto.Image.Length > 10 * 1024 * 1024)
            {
                    return BadRequest("Image size must not exceed 10 MB.");
            }

            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(createProductDto.Image.FileName, createProductDto.Image.OpenReadStream()),
                Folder = "product_images"
            };

            var uploadResult = await _cloudinary.UploadAsync(uploadParams);

            if (uploadResult.Error != null)
            {
                return BadRequest(uploadResult.Error.Message);
            }
            
            var product = createProductDto.ToProductFromCreateDto(uploadResult.SecureUrl.AbsoluteUri);

            await _productRepository.Post(product);
            return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
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
            var productModel = await _productRepository.Put(id, updateProductDto);
            if (productModel == null)
            {
                return NotFound();
            }
            return Ok(productModel);
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