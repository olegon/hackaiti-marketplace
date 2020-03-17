using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace Currency.Service.API.Services
{
    public class CurrencyCacheService : ICurrencyCacheService
    {
        private const string REDIS_CURRENCIES_KEY = "hackaiti-currencies";
        private readonly IZupCurrencyService _zupCurrencyService;
        private readonly IDatabase _database;

        public CurrencyCacheService(IZupCurrencyService currencyHttpClient, IDatabase database)
        {
            _zupCurrencyService = currencyHttpClient;
            _database = database;
        }

        public async Task<IEnumerable<Entities.Currency>> GetCurrencies()
        {
            var redisValue = _database.StringGet(REDIS_CURRENCIES_KEY);

            if (!redisValue.HasValue)
            {
                var currencies = await _zupCurrencyService.GetCurrencies();

                var timespanUntilMidnight = DateTime.Now.Date.AddDays(1).Subtract(DateTime.Now);

                _database.StringSet(REDIS_CURRENCIES_KEY, JsonConvert.SerializeObject(currencies), timespanUntilMidnight);

                return currencies;
            }

            return JsonConvert.DeserializeObject<IEnumerable<Entities.Currency>>(redisValue.ToString());
        }
    }
}