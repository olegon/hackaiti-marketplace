using System.Reflection;
using Jaeger;
using Jaeger.Samplers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OpenTracing;
using OpenTracing.Util;

namespace Cart.Service.API.Infrastructure.OpenTracing
{
    public static class JaegerOpenTracingConfiguration
    {
        public static IServiceCollection AddJaegerOpenTracing(this IServiceCollection services)
        {
            services.AddSingleton<ITracer>(serviceProvider =>
            {
                var serviceName = Assembly.GetEntryAssembly().GetName().Name;

                var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();

                var tracer =  Configuration.FromEnv(loggerFactory).GetTracer();

                GlobalTracer.Register(tracer);

                return tracer;
            });

            services.AddOpenTracing();

            return services;
        }
    }
}