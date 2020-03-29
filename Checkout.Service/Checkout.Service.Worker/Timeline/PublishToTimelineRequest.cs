using Checkout.Service.Worker.Models;
using MediatR;

namespace Checkout.Service.Worker.Timeline
{
    public class PublishToTimelineRequest : IRequest<PublishToTimelineResponse>
    {
        public StartCheckoutQueueMessage StartCheckoutMessage { get; set; }
        public CartInvoiceRequest InvoiceRequest { get; set; }        
    }
}