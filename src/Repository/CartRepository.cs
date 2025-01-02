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
            _httpContextAccessor.HttpContext!.Response.Cookies.Append("ShoppingCart", cartCookieValue, new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTimeOffset.Now.AddHours(1)
            });
        }
        public async Task<Cart> GetCart()
        {
            var cartCookie = _httpContextAccessor.HttpContext!.Request.Cookies["ShoppingCart"];
            if (string.IsNullOrEmpty(cartCookie))
            {
                return new Cart(); // Si no hay carrito, devolvemos uno vacío.
            }

            var cartItems = cartCookie.Split(',').Select(item => item.Split(':'))
                .Select(parts => new CartItem
                {
                    Product = new Product { Id = int.Parse(parts[0]) },
                    Quantity = int.Parse(parts[1])
                }).ToList();

            foreach (var cartItem in cartItems)
            {
                var product = await _productRepository.GetById(cartItem.Product.Id);
                if (product != null)
                {
                    cartItem.Product = product;
                }
            }
            return new Cart { Items = cartItems };
        }
        public async Task AddProductToCart(int productId)
        {
            var cart = await GetCart();
            var existingItem = cart.Items.FirstOrDefault(i => i.Product.Id == productId);

            if (existingItem != null)
            {
                existingItem.Quantity++;
            }
            else
            {
                cart.Items.Add(new CartItem 
                { 
                    Product = new Product 
                    { 
                        Id = productId, 
                    }, 
                    Quantity = 1 
                });
            }

            SetCartCookie(cart);
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

        public Task AddToCart(int productId)
        {
            throw new NotImplementedException();
        }

        public Task RemoveFromCart(int productId)
        {
            throw new NotImplementedException();
        }
    }
}