using System.Collections.Generic;
using System.Threading.Tasks;
using Refit;

namespace Currency.Service.API.Services
{
    public interface IZupCurrencyService
    {
        [Get("/currencies")]
        Task<IEnumerable<Entities.Currency>> GetCurrencies();
    }
}
