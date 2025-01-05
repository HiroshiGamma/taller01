using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.src.Dtos.Cart;
using api.src.Dtos.Receipt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using taller01.src.Dtos.Cart;
using taller01.src.Interface;

namespace taller01.src.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class CartController : ControllerBase
    {
        private readonly ICartRepository _cartRepository;
        private readonly IProductRepository _productRepository;
        private readonly IReceiptService _receiptService;
        public CartController(ICartRepository cartRepository, IProductRepository productRepository, IReceiptService receiptService)
        {
            _cartRepository = cartRepository;
            _productRepository = productRepository;
            _receiptService = receiptService;
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
        public async Task<IActionResult> AddToCartAsync([FromBody] int productId)
        {
            try
            {
                Console.WriteLine($"Adding product {productId} to cart"); // Debug line
                await _cartRepository.AddToCart(productId);
                var cart = await _cartRepository.GetCart(); // Get updated cart
                Console.WriteLine($"Cart now has {cart.Items.Count} items"); // Debug line
                return Ok(new { message = "Product added to cart", cartItemCount = cart.Items.Count });
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
        [HttpPost("checkout")]
        [Authorize(Roles = "User") ]
        public async Task<IActionResult> Checkout([FromForm] CheckoutDto checkoutDto)
        {
            try
            {
                var receipt = await _receiptService.CreateReceipt(
                    checkoutDto.UserRut,
                    checkoutDto.Country,
                    checkoutDto.City,
                    checkoutDto.Commune,
                    checkoutDto.Street
                );

                var receiptDto = new ReceiptDto
                {
                    Id = receipt.Id,
                    UserRut = receipt.UserRut,
                    Country = receipt.Country,
                    City = receipt.City,
                    Commune = receipt.Commune,
                    Street = receipt.Street,
                    Date = receipt.Date,
                    Items = receipt.Items.Select(i => new ReceiptItemDto
                    {
                        ProductId = i.ProductId,
                        Quantity = i.Quantity,
                        Price = i.Price,
                        Total = i.Total
                    }).ToList(),
                    Total = receipt.Total
                };
                await _cartRepository.ClearCart();
                return Ok(receiptDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}