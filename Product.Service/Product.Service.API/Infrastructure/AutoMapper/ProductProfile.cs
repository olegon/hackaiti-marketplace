using AutoMapper;
using Product.Service.API.Model;

namespace Product.Service.API.Infrastrcture.AutoMapper
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<CreateProductRequest, Entities.Product>();
            CreateMap<CreateProductRequest.ProductPrice, Entities.ProductPrice>();
            
            CreateMap<Entities.Product, CreateProductResponse>();
            CreateMap<Entities.ProductPrice, CreateProductResponse.ProductPrice>();
        }
    }
}