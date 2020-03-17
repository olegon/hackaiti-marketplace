namespace Currency.Service.API.Entities
{
    public class Currency
    {
        public string CurrencyCode { get; set; }
        public long CurrencyValue { get; set; }
        public long Scale { get; set; }
    }
}