using System.Collections.Generic;

namespace Currency.Service.API.Model
{
    public class GetCurrenciesResponse
    {
        public IEnumerable<Currency> Currencies { get; set; }

        public class Currency
        {
            public string CurrencyCode { get; set; }
            public long CurrencyValue { get; set; }
            public long Scale { get; set; }
        }
    }
}
