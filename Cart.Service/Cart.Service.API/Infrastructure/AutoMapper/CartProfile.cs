using AutoMapper;
using Cart.Service.API.Models;

namespace Cart.Service.API.Infrastrcture.AutoMapper
{
    public class CartProfile : Profile
    {
        public CartProfile()
        {
            this.CreateMap<Entities.Cart, CartResponse>();
            this.CreateMap<Entities.CartItem, CartResponse.CartItem>()
                .ForMember(dest => dest.CurrencyCode, opts => opts.MapFrom(src => src.Product.Price.CurrencyCode))
                .ForMember(dest => dest.Id, opts => opts.MapFrom(src => src.Product.Id));
        }
    }
}