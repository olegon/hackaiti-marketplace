using System;
using System.Threading;
using System.Threading.Tasks;
using Checkout.Service.Worker.Currency;
using Checkout.Service.Worker.Models;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Checkout.Service.Worker.Invoice
{
    public class CalculateInvoiceTotalHandler : IRequestHandler<CalculateInvoiceTotalRequest, CalculateInvoiceTotalResponse>
    {
        private readonly ILogger<CalculateInvoiceTotalHandler> _logger;
        private readonly IMediator _mediator;

        public CalculateInvoiceTotalHandler(ILogger<CalculateInvoiceTotalHandler> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        public async Task<CalculateInvoiceTotalResponse> Handle(CalculateInvoiceTotalRequest request, CancellationToken cancellationToken)
        {
            var getCurrenciesResponse = await _mediator.Send(new GetCurrenciesRequest());

            var currentCurrencies = getCurrenciesResponse.CurrentCurrencies;

            _logger.LogInformation("Current currencies values: {@currencies}", currentCurrencies);

            var total = new CartInvoiceRequest.CartTotal()
            {
                Amount = 0,
                Scale = 2,
                CurrencyCode = request.StartCheckoutMessage.CurrencyCode
            };

            foreach (var item in request.StartCheckoutMessage.Items)
            {
                var factor = currentCurrencies.Factors[$"{item.CurrencyCode}_TO_{request.StartCheckoutMessage.CurrencyCode}"];

                var currencPrice = item.Price / Math.Pow(10, item.Scale);
                var desidedPrice = currencPrice * factor * 100;

                total.Amount += (int)desidedPrice;
            }

            _logger.LogInformation("Cart total: {@total}", total);

            return new CalculateInvoiceTotalResponse()
            {
                Total = total
            };
        }
    }
}