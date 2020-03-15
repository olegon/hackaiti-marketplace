using System.Collections.Generic;

namespace Cart.Service.API.Models
{
    public class CartQueueMessage
    {
        public string Id { get; set; }
        public string CustomerId { get; set; }
        public string Status { get; set; }
        public string ControlId { get; set; }
        public IEnumerable<CartItem> Items { get; set; }

        public class CartItem
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public string ImageURL { get; set; }
            public int Price { get; set; }
            public int Scale { get; set; }
            public string CurrencyCode { get; set; }
        }
    }
}