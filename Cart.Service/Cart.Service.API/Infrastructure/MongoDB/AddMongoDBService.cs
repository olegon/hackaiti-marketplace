using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace Cart.Service.API.Infrastructure.MongoDB
{
    public static class AddMongoDbService
    {
        public static IServiceCollection AddMongoDB(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IMongoClient>(serviceProvider =>
            {
                return new MongoClient(configuration["MongoDbConnectionString"]);
            });

            services.AddScoped(serviceProvider =>
            {
                var client = serviceProvider.GetService<IMongoClient>();

                return client.GetDatabase("hackati_carts");
            });

            // services.AddTransient<IMongoCollection<Entities.Product>>(serviceProvider =>
            // {
            //     var database = serviceProvider.GetService<IMongoDatabase>();

            //     return database.GetCollection<Entities.Product>("products");
            // });

            return services;
        }
    }
}