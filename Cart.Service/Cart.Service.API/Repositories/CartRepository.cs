using System;
using System.Collections.Generic;
using System.Linq;
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
        public const string STATUS_DONE = "DONE";
        public const string STATUS_CANCELED = "CANCEL";
        public const string STATUS_PENDING = "PENDING";

        private readonly IMongoCollection<Entities.Cart> _cartsCollection;
        private readonly IProductService _productService;
        
        public CartRepository(IMongoCollection<Entities.Cart> cartsCollection, IProductService productService)
        {
            _cartsCollection = cartsCollection;
            _productService = productService;
        }

        public async Task<Entities.Cart> GetCartById(string cartId)
        {
            var databaseCarts = await _cartsCollection.FindAsync(cart => cart.Id == cartId);
            var databaseCart = databaseCarts.SingleOrDefault();

            if (databaseCart == null)
            {
                throw new CartNotFoundException();
            }

            return databaseCart;
        }

        public async Task<Entities.Cart> CancelCart(string cartId)
        {
            var databaseCart = await GetCartById(cartId);

            databaseCart.Status = STATUS_CANCELED;

            await _cartsCollection.ReplaceOneAsync(cart => cart.Id == cartId, databaseCart);

            return databaseCart;
        }

        public async Task<Entities.Cart> CreateCart(CreateCartRequest payload)
        {
            var product = await GetProductBySKU(payload.Item.SKU);

            var cart = new Entities.Cart()
            {
                CustomerId = payload.CustomerID,
                Status = STATUS_PENDING,
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

        private async Task<Product> GetProductBySKU(string sku)
        {
            try
            {
                return await _productService.GetProductBySku(sku);
            }
            catch (ValidationApiException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
            {
                throw new ProductNotFound();
            }
        }

        public async Task<Entities.Cart> UpdateCartItem(string cartId, UpdateCartItemRequest payload)
        {
            var databaseCart = await GetCartById(cartId);

            if (databaseCart.Status != STATUS_PENDING)
            {
                throw new InvalidCartException($"Cart {databaseCart.Id} has status {databaseCart.Status}");
            }

            var requiredCartItem = databaseCart.Items.SingleOrDefault(item => item.Product.SKU == payload.SKU);

            if (requiredCartItem == null)
            {
                var product = await GetProductBySKU(payload.SKU);

                var newCartItems = new List<CartItem>(databaseCart.Items);

                newCartItems.Add(new CartItem()
                {
                    Price = product.Price.Amount * payload.Quantity,
                    Quantity = payload.Quantity,
                    Scale = product.Price.Scale,
                    Product = product
                });

                databaseCart.Items = newCartItems;
            }
            else
            {
                requiredCartItem.Quantity += payload.Quantity;
                requiredCartItem.Price = requiredCartItem.Product.Price.Amount * requiredCartItem.Quantity;

                if (requiredCartItem.Quantity == 0)
                {
                    databaseCart.Items = databaseCart.Items.Where(cartItem => cartItem.Product.SKU != payload.SKU);
                }
                else if (requiredCartItem.Quantity < 0)
                {
                    throw new InvalidCartException($"Item SKU {payload.SKU} has less than 0 quantity");
                }
            }

            await _cartsCollection.ReplaceOneAsync(cart => cart.Id == cartId, databaseCart);

            return databaseCart;
        }

        public async Task<Entities.Cart> CartCheckout(string cartId, CartCheckoutRequest payload)
        {
            var databaseCart = await GetCartById(cartId);

            if (databaseCart.Status != STATUS_PENDING)
            {
                throw new InvalidCartException($"Cart {databaseCart.Id} has status {databaseCart.Status}");
            }

            databaseCart.Status = STATUS_DONE;

            await _cartsCollection.ReplaceOneAsync(cart => cart.Id == cartId, databaseCart);

            return databaseCart;
        }
    }
}