using System.Threading.Tasks;
using Cart.Service.API.Models;

namespace Cart.Service.API.Repositories
{
    public interface ICartRepository
    {
        Task<Entities.Cart> CreateCart(CreateCartRequest payload);
        Task<Entities.Cart> CancelCart(string cartId);
        Task<Entities.Cart> UpdateCartItem(string cartId, UpdateCartItemRequest payload);
        Task<Entities.Cart> CartCheckout(string cartId, CartCheckoutRequest payload, string controlId);
    }
}