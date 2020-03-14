using System;
using System.Net;
using System.Threading.Tasks;
using Cart.Service.API.Entities;
using Cart.Service.API.Exceptions;
using Cart.Service.API.Models;
using Cart.Service.API.Services;
using MongoDB.Driver;
using Refit;

namespace Cart.Service.API.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly IMongoCollection<Entities.Cart> _cartsCollection;
        private readonly IProductService _productService;
        public CartRepository(IMongoCollection<Entities.Cart> cartsCollection, IProductService productService)
        {
            _cartsCollection = cartsCollection;
            _productService = productService;
        }

        public async Task<Entities.Cart> CreateCart(CreateCartRequest payload)
        {
            var product = await GetProduct(payload);

            if (product == null)
            {
                throw new ProductNotFound();
            }

            var cart = new Entities.Cart()
            {
                CustomerId = payload.CustomerID,
                Status = "PENDING",
                Items = new[]
                {
                    new Entities.CartItem()
                    {
                        Price = product.Price.Amount * payload.Item.Quantity,
                        Scale = product.Price.Scale,
                        Quantity = payload.Item.Quantity,
                        Product = product
                    }
                }
            };

            _cartsCollection.InsertOne(cart);

            return cart;
        }

        private async Task<Product> GetProduct(CreateCartRequest payload)
        {
            try
            {
                return await _productService.GetProductBySku(payload.Item.SKU);
            }
            catch (ValidationApiException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }
        }
    }
}