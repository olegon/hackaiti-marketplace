using System.Threading.Tasks;
using Checkout.Service.Worker.Models;
using Refit;

namespace Checkout.Service.Worker.Services
{
    public interface IHttpCurrencyService
    {
        [Get("/currencies")]
        Task<GetHttpCurrenciesResponse> GetCurrencies();
    }
}