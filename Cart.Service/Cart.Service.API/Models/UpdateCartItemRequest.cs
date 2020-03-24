namespace Cart.Service.API.Models
{
    public class UpdateCartItemRequest
    {
        public string SKU { get; set; }
        public int Quantity { get; set; }
    }
}