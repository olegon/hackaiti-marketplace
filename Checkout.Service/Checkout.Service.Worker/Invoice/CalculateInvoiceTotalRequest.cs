using Checkout.Service.Worker.Models;
using MediatR;

namespace Checkout.Service.Worker.Invoice
{
    public class CalculateInvoiceTotalRequest : IRequest<CalculateInvoiceTotalResponse>
    {
        public StartCheckoutQueueMessage StartCheckoutMessage { get; internal set; }
    }
}