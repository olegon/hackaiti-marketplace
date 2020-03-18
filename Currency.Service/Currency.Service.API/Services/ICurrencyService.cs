using System.Collections.Generic;
using System.Threading.Tasks;
using Currency.Service.API.Model;

namespace Currency.Service.API.Services
{
    public interface ICurrencyService
    {
        Task<GetCurrenciesResponse> GetCurrencies();
    }
}