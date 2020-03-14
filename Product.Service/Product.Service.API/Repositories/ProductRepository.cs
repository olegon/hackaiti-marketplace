using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;
using Product.Service.API.Exceptions;

namespace Product.Service.API.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly IMongoCollection<Entities.Product> _productsCollection;

        public ProductRepository(IMongoCollection<Entities.Product> productsCollection)
        {
            this._productsCollection = productsCollection;

        }

        public async Task AddProduct(Entities.Product product)
        {
            var productDatabase = await GetProductBySku(product.SKU);

            if (productDatabase != null)
            {
                throw new DuplicatedSkuException($"Já existe um produto com o SKU {product.SKU}");
            }

            await _productsCollection.InsertOneAsync(product);
        }

        public async Task<IEnumerable<Entities.Product>> GetAllProducts()
        {
            var databaseProducts = await _productsCollection.FindAsync(product => true);

            return databaseProducts.ToEnumerable();
        }

        public async Task<Entities.Product> GetProductById(string id)
        {
            try
            {
                var databaseProducts = await  _productsCollection.FindAsync(product => product.Id == id);
                return databaseProducts.SingleOrDefault();
            }
            catch (FormatException) // Find by an invalid ObjectId
            {
                return null;
            }
        }

         public async Task<Entities.Product> GetProductBySku(string sku)
        {
            var databaseProducts = await  _productsCollection.FindAsync(product => product.SKU == sku);
            
            return databaseProducts.SingleOrDefault();
        }
    }
}