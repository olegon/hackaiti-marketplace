using AutoMapper;
using Checkout.Service.Worker.Models;

namespace Checkout.Service.Worker.Infrastructure.AutoMapper
{
    public class CheckoutProfile : Profile
    {
        public CheckoutProfile()
        {
            CreateMap<StartCheckoutQueueMessage, CartInvoiceRequest>();
            CreateMap<StartCheckoutQueueMessage.CartItem, CartInvoiceRequest.CartItem>();
        }
    }
}