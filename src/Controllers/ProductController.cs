using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.src.data;
using api.src.Dtos;
using api.src.Mappers;
using api.src.models;
using CloudinaryDotNet;
using Microsoft.AspNetCore.Mvc;
using taller01.src.Interfaces;
using taller01.src.Mappers;
using taller01.src.Repository;

namespace taller01.src.Controllers
{
    [Route("product")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        private readonly IUserRepository _userRepository;
        private readonly Cloudinary _cloudinary;
        public ProductController(IProductRepository productRepository, IUserRepository userRepository, Cloudinary cloudinary)
        {
            _productRepository = productRepository;
            _userRepository = userRepository;
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


    [HttpPost]
    public IActionResult post([FromBody] CreateProductDto createProductDto)
    {
        var product = createProductDto.ToProductFromCreateDto();
        _productRepository.Post(product);
        return CreatedAtAction(nameof(Get), new { id = product.Id }, product.ToProductDto());
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

    [HttpDelete("{id}")]
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