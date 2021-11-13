using System.Collections.Generic;

namespace Currency.Service.API.Model
{
    public record class GetCurrenciesResponse(IEnumerable<Currency> Currencies, IDictionary<string, double> Factors);

    public record class Currency(string CurrencyCode, int CurrencyValue, int Scale);
}
