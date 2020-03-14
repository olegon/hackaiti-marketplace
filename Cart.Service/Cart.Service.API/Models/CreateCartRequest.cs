namespace Cart.Service.API.Models
{
    public class CreateCartRequest
    {
        public string CustomerID { get; set; }
        public CartItem Item { get; set; }

        public class CartItem
        {
            public string SKU { get; set; }
            public int Quantity { get; set; }
        }
    }
}