using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Currency.Service.API.Model;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace Currency.Service.API.Services
{
    public class CurrencyService : ICurrencyService
    {
        private const string REDIS_CURRENCIES_KEY = "hackaiti-currencies";
        private readonly IZupCurrencyService _zupCurrencyService;
        private readonly IDatabase _database;
        private readonly IMapper _mapper;

        public CurrencyService(IZupCurrencyService currencyHttpClient, IDatabase database, IMapper mapper)
        {
            _zupCurrencyService = currencyHttpClient;
            _database = database;
            _mapper = mapper;
        }

        public async Task<GetCurrenciesResponse> GetCurrencies()
        {
            var redisValue = _database.StringGet(REDIS_CURRENCIES_KEY);

            if (!redisValue.HasValue)
            {
                var currencies = await _zupCurrencyService.GetCurrencies();

                var result = new GetCurrenciesResponse()
                {
                    Currencies = _mapper.Map<IEnumerable<GetCurrenciesResponse.Currency>>(currencies),
                    Factors = CalculateFactors(currencies)
                };

                var timespanUntilMidnight = DateTime.Now.Date.AddDays(1).Subtract(DateTime.Now);

                _database.StringSet(REDIS_CURRENCIES_KEY, JsonConvert.SerializeObject(result), timespanUntilMidnight);

                return result;
            }

            return JsonConvert.DeserializeObject<GetCurrenciesResponse>(redisValue.ToString());
        }

        public IDictionary<string, double> CalculateFactors(IEnumerable<Entities.Currency> currencies)
        {
            var usd2brl = currencies.Single(t => t.CurrencyCode?.ToUpper() == "USD_TO_BRL");
            var usd2eur = currencies.Single(t => t.CurrencyCode?.ToUpper() == "USD_TO_EUR");

            var usd2brl_factor = GetFactor(usd2brl.CurrencyValue, usd2brl.Scale);
            var usd2eur_factor = GetFactor(usd2eur.CurrencyValue, usd2eur.Scale);
            var brl2usd_factor = 1 / usd2brl_factor;
            var eur2usd_factor = 1 / usd2eur_factor;
            var brl2eur_factor = brl2usd_factor * usd2eur_factor;
            var eur2brl_factor = eur2usd_factor * usd2brl_factor;

            return new Dictionary<string, double>()
            {
                { "USD_TO_BRL", usd2brl_factor },
                { "USD_TO_EUR", usd2eur_factor },
                { "BRL_TO_USD", brl2usd_factor },
                { "EUR_TO_USD", eur2usd_factor },
                { "BRL_TO_EUR", brl2eur_factor },
                { "EUR_TO_BRL", eur2brl_factor },
            };
        }

        public double GetFactor(int amount, int scale)
        {
            return 1.0 * amount / Math.Pow(10, scale);
        }
    }
}