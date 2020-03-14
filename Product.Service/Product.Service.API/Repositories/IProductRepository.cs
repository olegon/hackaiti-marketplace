using System.Collections.Generic;
using System.Threading.Tasks;

namespace Product.Service.API.Repositories
{
    public interface IProductRepository
    {
        Task AddProduct(Entities.Product product);
        Task<IEnumerable<Entities.Product>> GetAllProducts();
        Task<Entities.Product> GetProductById(string id);
        Task<Entities.Product> GetProductBySku(string sku);
    }
}