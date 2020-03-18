namespace Currency.Service.API.Entities
{
    public class Currency
    {
        public string CurrencyCode { get; set; }
        public int CurrencyValue { get; set; }
        public int Scale { get; set; }
    }
}