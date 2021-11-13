using AutoMapper;

namespace Currency.Service.API.Infrastructure.AutoMapper
{
    public class CurrencyProfile : Profile
    {
        public CurrencyProfile()
        {
            CreateMap<Entities.Currency, Model.Currency>();
        }
    }
}