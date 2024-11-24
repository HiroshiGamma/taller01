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
using Microsoft.AspNetCore.Mvc;
using taller01.src.Interfaces;
using taller01.src.Mappers;
using taller01.src.Repository;

namespace taller01.src.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        private readonly Cloudinary _cloudinary;
        public ProductController(IProductRepository productRepository,  Cloudinary cloudinary)
        {
            _productRepository = productRepository;
            _cloudinary = cloudinary;
        }
    
    [HttpGet]
    public IActionResult Get(string? searchText, string? type, string? order)
    {
        var productsQuery = _productRepository.GetAsQuery(searchText, type, order);

        if (!string.IsNullOrEmpty(searchText))
        {
            productsQuery = productsQuery.Where(p => p.Name.Contains(searchText));
        }

        if (!string.IsNullOrEmpty(type))
        {
            productsQuery = productsQuery.Where(p => p.Type == type);
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
    [HttpGet("{id: int}")]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        var product = await _productRepository.GetById(id);

        if (product == null)
        {
            return NotFound();
        }
        return Ok(product);
    }


    [HttpPost]
    [Consumes("multipart/form-data")]
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

    [HttpPut]
    [Route("{id:int}")]
    public async Task<IActionResult> Put([FromRoute] int id, [FromBody] UpdateProductDto updateProductDto)
    {
        var productModel = await _productRepository.Put(id, updateProductDto);
        if (productModel == null)
        {
            return NotFound();
        }
        return Ok(productModel);
    }

    [HttpDelete("{id: int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var product = await _productRepository.Delete(id);
        if (product == null)
        {
            return NotFound();
        }
        return NoContent();
    }

    

  


    
    /* probando
    [HttpPost("add-to-cart")]
    public IActionResult AddToCart(int productId, int userId)
    {
        var user = _userRepository.GetById(userId);
        var product = _productRepository.GetById(productId);

        if (user == null || product == null || product. <= 0)
        {
            return BadRequest("Invalid user or product, or product out of stock.");
        }

        
        _userRepository.AddProductToUser(userId, product);
        product.Stock--;

        _context.SaveChanges();

        return Ok("Product added to cart.");
    } */
    }
    
}