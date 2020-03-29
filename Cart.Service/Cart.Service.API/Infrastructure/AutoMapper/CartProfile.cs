using AutoMapper;
using Cart.Service.API.Models;

namespace Cart.Service.API.Infrastructure.AutoMapper
{
    public class CartProfile : Profile
    {
        public CartProfile()
        {
            this.CreateMap<Entities.Cart, CartResponse>();
            this.CreateMap<Entities.CartItem, CartResponse.CartItem>()
                .ForMember(dest => dest.CurrencyCode, opts => opts.MapFrom(src => src.Product.Price.CurrencyCode))
                .ForMember(dest => dest.Id, opts => opts.MapFrom(src => src.Product.Id));

            this.CreateMap<Entities.Cart, StartCheckoutQueueMessage>();
            this.CreateMap<Entities.CartItem, StartCheckoutQueueMessage.CartItem>()
                .ForMember(dest => dest.Id, opts => opts.MapFrom(src => src.Product.Id))
                .ForMember(dest => dest.Name, opts => opts.MapFrom(src => src.Product.Name))
                .ForMember(dest => dest.ImageURL, opts => opts.MapFrom(src => src.Product.ImageURL))
                .ForMember(dest => dest.CurrencyCode, opts => opts.MapFrom(src => src.Product.Price.CurrencyCode));
        }
    }
}