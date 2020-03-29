using AutoMapper;
using Product.Service.API.Model;

namespace Product.Service.API.Infrastructure.AutoMapper
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<CreateProductRequest, Entities.Product>();
            CreateMap<CreateProductRequest.ProductPrice, Entities.ProductPrice>();
            
            CreateMap<Entities.Product, ProductResponse>();
            CreateMap<Entities.ProductPrice, ProductResponse.ProductPrice>();
        }
    }
}