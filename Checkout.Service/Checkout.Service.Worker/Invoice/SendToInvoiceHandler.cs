using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Checkout.Service.Worker.Models;
using Checkout.Service.Worker.Services;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Checkout.Service.Worker.Invoice
{
    public class SendToInvoiceHandler : IRequestHandler<SendToInvoiceRequest, SendToInvoiceResponse>
    {
        private readonly ILogger<SendToInvoiceHandler> _logger;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly IHttpInvoiceService _invoiceService;

        public SendToInvoiceHandler(ILogger<SendToInvoiceHandler> logger, IMediator mediator, IMapper mapper, IHttpInvoiceService invoiceService)
        {
            _logger = logger;
            _mediator = mediator;
            _mapper = mapper;
            _invoiceService = invoiceService;
        }

        public async Task<SendToInvoiceResponse> Handle(SendToInvoiceRequest request, CancellationToken cancellationToken)
        {
            var calculateInvoiceTotalResponse = await _mediator.Send(new CalculateInvoiceTotalRequest()
            {
                StartCheckoutMessage = request.StartCheckoutMessage
            });

            var invoiceRequest = _mapper.Map<CartInvoiceRequest>(request.StartCheckoutMessage);
            invoiceRequest.Total = calculateInvoiceTotalResponse.Total;

            _logger.LogInformation("Posting invoice: {@invoice}", invoiceRequest);

            await _invoiceService.SendInvoice(invoiceRequest, request.StartCheckoutMessage.ControlId);

            return new SendToInvoiceResponse()
            {
                InvoiceRequest = invoiceRequest
            };
        }
    }
}