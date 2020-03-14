using System.Threading.Tasks;
using Cart.Service.API.Models;

namespace Cart.Service.API.Repositories
{
    public interface ICartRepository
    {
        Task<Entities.Cart> CreateCart(CreateCartRequest payload);
    }
}