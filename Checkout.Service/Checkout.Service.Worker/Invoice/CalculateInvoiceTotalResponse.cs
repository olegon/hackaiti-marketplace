using Checkout.Service.Worker.Models;

namespace Checkout.Service.Worker.Invoice
{
    public class CalculateInvoiceTotalResponse
    {
        public CartInvoiceRequest.CartTotal Total { get; set; }
    }
}