using Checkout.Service.Worker.Models;

namespace Checkout.Service.Worker.Invoice
{
    public class SendToInvoiceResponse
    {
        public CartInvoiceRequest InvoiceRequest { get; internal set; }
    }
}