using System.Collections.Generic;

namespace Checkout.Service.Worker.Models
{
    public class GetHttpCurrenciesResponse
    {
        public IEnumerable<Currency> Currencies { get; set; }
        public IDictionary<string, double> Factors { get; set; }

        public class Currency
        {
            public string CurrencyCode { get; set; }
            public int CurrencyValue { get; set; }
            public int Scale { get; set; }
        }
    }
}