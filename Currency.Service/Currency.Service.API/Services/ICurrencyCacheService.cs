using System.Collections.Generic;
using System.Threading.Tasks;

namespace Currency.Service.API.Services
{
    public interface ICurrencyCacheService
    {
        Task<IEnumerable<Entities.Currency>> GetCurrencies();
    }
}