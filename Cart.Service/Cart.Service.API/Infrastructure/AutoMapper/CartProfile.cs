using AutoMapper;
using Cart.Service.API.Models;

namespace Cart.Service.API.Infrastrcture.AutoMapper
{
    public class CartProfile : Profile
    {
        public CartProfile()
        {
            this.CreateMap<Entities.Cart, CreateCartResponse>();
            this.CreateMap<Entities.CartItem, CreateCartResponse.CartItem>()
                .ForMember(dest => dest.CurrencyCode, opts => opts.MapFrom(src => src.Product.Price.CurrencyCode));
        }
    }
}