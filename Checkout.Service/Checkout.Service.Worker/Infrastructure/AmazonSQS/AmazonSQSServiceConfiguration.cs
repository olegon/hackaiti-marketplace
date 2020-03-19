using Amazon.SQS;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Checkout.Service.Worker.Infrastructure.AmazonSQS
{
    public static class AmazonSQSServiceConfiguration
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