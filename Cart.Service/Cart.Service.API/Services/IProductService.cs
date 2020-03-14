using System.Threading.Tasks;
using Refit;

namespace Cart.Service.API.Services
{
    public interface IProductService
    {
        [Get("/products/sku/{sku}")]
        Task<Entities.Product> GetProductBySku(string sku);
    }
}