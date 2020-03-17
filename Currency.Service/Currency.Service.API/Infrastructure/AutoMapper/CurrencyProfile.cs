using AutoMapper;
using Currency.Service.API.Model;

namespace Currency.Service.API.Infrastructure.AutoMapper
{
    public class CurrencyProfile : Profile
    {
        public CurrencyProfile()
        {
            CreateMap<Entities.Currency, GetCurrenciesResponse.Currency>();
        }
    }
}