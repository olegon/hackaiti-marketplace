using System.Threading;
using System.Threading.Tasks;
using Checkout.Service.Worker.Services;
using MediatR;

namespace Checkout.Service.Worker.Currency
{
    public class GetCurrenciesHandler : IRequestHandler<GetCurrenciesRequest, GetCurrenciesResponse>
    {
        private readonly IHttpCurrencyService _currencyService;

        public GetCurrenciesHandler(IHttpCurrencyService currencyService)
        {
            this._currencyService = currencyService;
        }

        public async Task<GetCurrenciesResponse> Handle(GetCurrenciesRequest request, CancellationToken cancellationToken)
        {
            var currentCurrencies = await _currencyService.GetCurrencies();

            return new GetCurrenciesResponse()
            {
                CurrentCurrencies = currentCurrencies
            };
        }
    }
}