using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Amazon.SQS;

namespace Cart.Service.API.Infrastructure.AmazonSQS
{
    public static class AddAmazonSQSService
    {
        public static IServiceCollection AddAmazonSQS(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<AmazonSQSClient>(serviceProvider =>
            {
                return new AmazonSQSClient();
            });

            return services;
        }
    }
}