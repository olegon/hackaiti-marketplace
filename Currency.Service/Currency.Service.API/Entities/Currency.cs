namespace Currency.Service.API.Entities
{
    public record class Currency(string CurrencyCode, int CurrencyValue, int Scale);
}