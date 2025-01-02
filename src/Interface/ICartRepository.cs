using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.src.models;

namespace taller01.src.Interface
{
    public interface ICartRepository
    {
        public void SetCartCookie(Cart cart);
        public Task<Cart> GetCart();
        public Task AddToCart(int productId);
        public Task RemoveFromCart(int productId);
    }
}