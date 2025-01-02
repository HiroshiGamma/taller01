using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using taller01.src.Interface;

namespace taller01.src.Service
{
    public class ReceiptService : IReceiptService
    {
        private readonly IProductRepository _productRepository;
        private readonly ICartRepository _cartRepository;
        private readonly IReceiptRepository _receiptRepository;
        public ReceiptService(IProductRepository productService, ICartRepository cartRepository, IReceiptRepository receiptRepository)
        {
            _productRepository = productService;
            _cartRepository = cartRepository;
            _receiptRepository = receiptRepository;
        }

        public async Task<Receipt> CreateReceipt(string rut, string country, string city, string commune, string street)
        {
            var cart = await _cartRepository.GetCart();
            if (!cart.Items.Any())
            {
                throw new InvalidOperationException("El carrito está vacío.");
            }

            var receipt = new Receipt
            {
                UserRut = rut,
                Country = country,
                City = city,
                Commune = commune,
                Street = street,
                Date = DateTime.UtcNow,
                Items = new List<ReceiptItem>(),
                Total = 0
            };

            // Procesar los productos en el carrito
            foreach (var cartItem in cart.Items)
            {
                var product = await _productRepository.GetById(cartItem.Product.Id);
                if (product == null || product.Stock < cartItem.Quantity)
                {
                    throw new InvalidOperationException($"Producto {cartItem.Product.Name} no disponible en stock.");
                }

                // Crear item de la boleta
                var receiptItem = new ReceiptItem
                {
                    ProductId = product.Id,
                    Quantity = cartItem.Quantity,
                    Price = product.Price,
                    Total = product.Price * cartItem.Quantity
                };
                receipt.Items.Add(receiptItem);
                receipt.Total += receiptItem.Total;

                // Actualizar stock
                product.Stock -= cartItem.Quantity;
                await _productRepository.UpdateStock(product.Id, product.Stock);
            }

            // Guardar la boleta
            await _receiptRepository.Add(receipt);

            // Limpiar el carrito después de la compra
            //_cartRepository.ClearCart();

            return receipt;
        }
    }
}