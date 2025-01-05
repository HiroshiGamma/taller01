using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.src.models;
using taller01.src.Interface;

namespace taller01.src.Repository
{
    public class CartRepository : ICartRepository
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IProductRepository _productRepository;

        public CartRepository(IHttpContextAccessor httpContextAccessor, IProductRepository productRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _productRepository = productRepository;
        }
        public void SetCartCookie(Cart cart)
        {
            var cartCookieValue = string.Join(",", cart.Items.Select(i => $"{i.Product.Id}:{i.Quantity}"));
            Console.WriteLine($"Setting cookie with value: {cartCookieValue}"); // Debug line

            _httpContextAccessor.HttpContext!.Response.Cookies.Append("ShoppingCart", cartCookieValue, new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTimeOffset.Now.AddHours(1),
                SameSite = SameSiteMode.Lax,
                Path = "/",
                Secure = false,  // Set to true if using HTTPS
                Domain = null    // This will use the current domain
            });
        }
        public async Task<Cart> GetCart()
        {
            var httpContext = _httpContextAccessor.HttpContext!;
            Console.WriteLine("All cookies:");
            foreach (var cookie in httpContext.Request.Cookies)
            {
                Console.WriteLine($"{cookie.Key}: {cookie.Value}");
            }
            var cartCookie = _httpContextAccessor.HttpContext!.Request.Cookies["ShoppingCart"];
            Console.WriteLine($"Cart cookie value: {cartCookie}"); // Debug line

            if (string.IsNullOrEmpty(cartCookie))
            {
                Console.WriteLine("No cart cookie found"); // Debug line
                return new Cart(); 
            }

            try 
            {
                var cartItems = cartCookie.Split(',').Select(item => 
                {
                    var parts = item.Split(':');
                    Console.WriteLine($"Processing item - ID: {parts[0]}, Quantity: {parts[1]}"); // Debug line
                    return new CartItem
                    {
                        Product = new Product { Id = int.Parse(parts[0]) },
                        Quantity = int.Parse(parts[1])
                    };
                }).ToList();

                foreach (var cartItem in cartItems)
                {
                    var product = await _productRepository.GetById(cartItem.Product.Id);
                    if (product != null)
                    {
                        cartItem.Product = product;
                        Console.WriteLine($"Found product: {product.Name}"); // Debug line
                    }
                    else
                    {
                        Console.WriteLine($"Product not found for ID: {cartItem.Product.Id}"); // Debug line
                    }
                }

                var cart = new Cart { Items = cartItems };
                Console.WriteLine($"Returning cart with {cart.Items.Count} items"); // Debug line
                return cart;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing cart: {ex.Message}"); // Debug line
                return new Cart();
            }
        }
        public async Task AddProductToCart(int productId)
        {
            var cart = await GetCart();
            var product = await _productRepository.GetById(productId);
            
            if (product == null)
            {
                throw new Exception($"Product with ID {productId} not found");
            }

            var existingItem = cart.Items.FirstOrDefault(i => i.Product.Id == productId);

            if (existingItem != null)
            {
                existingItem.Quantity++;
                Console.WriteLine($"Increased quantity for product {productId}"); // Debug line
            }
            else
            {
                cart.Items.Add(new CartItem 
                { 
                    Product = product,
                    Quantity = 1 
                });
                Console.WriteLine($"Added new product {productId} to cart"); // Debug line
            }

            SetCartCookie(cart);
            Console.WriteLine("Cart cookie updated"); // Debug line
        }
        public async Task RemoveProductFromCart(int productId)
        {
            var cart = await GetCart();
            var existingItem = cart.Items.FirstOrDefault(i => i.Product.Id == productId);

            if (existingItem != null)
            {
                if (existingItem.Quantity > 1)
                {
                    existingItem.Quantity--;
                }
                else
                {
                    cart.Items.Remove(existingItem);
                }
            }
            SetCartCookie(cart);
        }

        public async Task AddToCart(int productId)
        {
            Console.WriteLine($"Adding product {productId} to cart"); // Debug line
            await AddProductToCart(productId);
        }

        public async Task RemoveFromCart(int productId)
        {
            Console.WriteLine($"Removing product {productId} from cart"); // Debug line
            await RemoveProductFromCart(productId);
        }
    }
}