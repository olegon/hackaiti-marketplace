using Checkout.Service.Worker.Models;
using MediatR;

namespace Checkout.Service.Worker.Invoice
{
    public class SendToInvoiceRequest : IRequest<SendToInvoiceResponse>
    {
        public StartCheckoutQueueMessage StartCheckoutMessage { get; internal set; }
    }
}