using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.src.data;
using Microsoft.AspNetCore.Mvc;
using taller01.src.Mappers;

namespace taller01.src.Controllers
{
    [Route("product")]
    [ApiController]
    public class ProductController : ControllerBase
    {
         private readonly ApplicationDBContext _context;
        public ProductController(ApplicationDBContext context)
        {
            _context = context;
        }
    
    [HttpGet]
    public IActionResult Get(string? searchText, string? type, string? order)
    {
        var productsQuery = _context.Products.AsQueryable();

        if (!string.IsNullOrEmpty(searchText))
        {
            productsQuery = productsQuery.Where(p => p.Nombre.Contains(searchText));
        }

        if (!string.IsNullOrEmpty(type))
        {
            productsQuery = productsQuery.Where(p => p.Tipo == type);
        }

        if (order == "asc")
        {
            productsQuery = productsQuery.OrderBy(p => p.Precio);
        }
        else if (order == "desc")
        {
            productsQuery = productsQuery.OrderByDescending(p => p.Precio);
        }

        var products = productsQuery
            .Where(p => p.Stock > 0)
            .ToList()
            .Select(p => p.ToProductDto());

        return Ok(products);
    }

    [HttpPost("add-to-cart")]
    public IActionResult AddToCart(int productId, int userId)
    {
        var user = _context.Users.Find(userId);
        var product = _context.Products.Find(productId);

        if (user == null || product == null || product.Stock <= 0)
        {
            return BadRequest("Invalid user or product, or product out of stock.");
        }

        user.Products.Add(product);
        product.Stock--;

        _context.SaveChanges();

        return Ok("Product added to cart.");
    }
    }
}