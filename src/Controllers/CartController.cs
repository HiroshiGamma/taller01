using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.src.Dtos.Cart;
using Microsoft.AspNetCore.Mvc;
using taller01.src.Interface;

namespace taller01.src.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class CartController : ControllerBase
    {
        private readonly ICartRepository _cartRepository;
        private readonly IProductRepository _productRepository;
        public CartController(ICartRepository cartRepository, IProductRepository productRepository)
        {
            _cartRepository = cartRepository;
            _productRepository = productRepository;
        }
        [HttpGet]
        public async Task<IActionResult> GetCartAsync()
        {
            try
            {
                var cart = await _cartRepository.GetCart();
                var cartDto = new CartDto
                {
                    Items = cart.Items.Select(item => new CartItemDto
                    {
                        ProductId = item.Product.Id,
                        ProductName = item.Product.Name,
                        Quantity = item.Quantity,
                        Price = item.Product.Price
                    }).ToList(),
                    Total = cart.Items.Sum(item => item.Product.Price * item.Quantity)
                };

                return Ok(cartDto);

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpPost("add")]
        public IActionResult AddToCart([FromBody] int productId)
        {
            try
            {
                var product = _productRepository.GetById(productId);
                if (product == null)
                {
                    return NotFound();
                }

                _cartRepository.AddToCart(productId);
                return Ok("Producto agregado al carrito");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpPost("remove")]
        public IActionResult RemoveFromCart([FromBody] int productId)
        {
            try
            {
                _cartRepository.RemoveFromCart(productId);
                return Ok($"Producto con {productId} eliminado del carrito");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}