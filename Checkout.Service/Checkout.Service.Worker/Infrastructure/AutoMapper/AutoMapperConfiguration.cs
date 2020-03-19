using AutoMapper;
using Microsoft.Extensions.DependencyInjection;

namespace Checkout.Service.Worker.Infrastructure.AutoMapper
{
    public static class AutoMapperConfiguration
    {
        public static IServiceCollection AddAutoMapper(this IServiceCollection services)
        {
            var mapperConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<CheckoutProfile>();
            });

            services.AddTransient<IMapper>(serviceProvider =>
            {
                return mapperConfiguration.CreateMapper();
            });

            return services;
        }
    }
}