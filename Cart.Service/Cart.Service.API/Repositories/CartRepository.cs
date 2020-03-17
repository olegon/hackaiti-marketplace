using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Amazon.SQS;
using Amazon.SQS.Model;
using AutoMapper;
using Cart.Service.API.Entities;
using Cart.Service.API.Exceptions;
using Cart.Service.API.Models;
using Cart.Service.API.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Newtonsoft.Json;
using Refit;

namespace Cart.Service.API.Repositories
{
    public class CartRepository : ICartRepository
    {
        public const string STATUS_DONE = "DONE";
        public const string STATUS_CANCELED = "CANCEL";
        public const string STATUS_PENDING = "PENDING";
        private readonly ILogger<CartRepository> _logger;
        private readonly IMapper _mapper;
        private readonly IMongoCollection<Entities.Cart> _cartsCollection;
        private readonly IProductService _productService;
        private readonly AmazonSQSClient _amazonSQSClient;
        private readonly IConfiguration _configuration;

        public CartRepository(
            ILogger<CartRepository> logger,
            IMapper mapper,
            IMongoCollection<Entities.Cart> cartsCollection,
            IProductService productService,
            AmazonSQSClient amazonSQSClient,
            IConfiguration configuration)
        {
            _logger = logger;
            _mapper = mapper;
            _cartsCollection = cartsCollection;
            _productService = productService;
            _amazonSQSClient = amazonSQSClient;
            _configuration = configuration;
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

        public async Task<Entities.Cart> CartCheckout(string cartId, CartCheckoutRequest payload, string controlId)
        {
            var databaseCart = await GetCartById(cartId);

            if (databaseCart.Status != STATUS_PENDING)
            {
                throw new InvalidCartException($"Cart {databaseCart.Id} has status {databaseCart.Status}");
            }

            databaseCart.ControlId = controlId;
            databaseCart.CurrencyCode = payload.CurrencyCode;
            databaseCart.Status = STATUS_DONE;

            var cartQueueMessage = _mapper.Map<StartCheckoutQueueMessage>(databaseCart);

            await _amazonSQSClient.SendMessageAsync(new SendMessageRequest()
            {
                QueueUrl = _configuration["AmazonSQSCheckoutQueueURL"],
                MessageBody = JsonConvert.SerializeObject(cartQueueMessage)
            });

            await _cartsCollection.ReplaceOneAsync(cart => cart.Id == cartId, databaseCart);

            return databaseCart;
        }
    }
}