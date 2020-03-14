namespace Product.Service.API.Model
{
    public class CreateProductRequest
    {
        public string SKU { get; set; }
        public string Name { get; set; }
        public string ShortDescription { get; set; }
        public string LongDescription { get; set; }
        public string ImageURL { get; set; }
        public ProductPrice Price { get; set; }

        public class ProductPrice
        {
            public int Amount { get; set; }
            public int Scale { get; set; }
            public string CurrencyCode { get; set; }
        }
    }
}
