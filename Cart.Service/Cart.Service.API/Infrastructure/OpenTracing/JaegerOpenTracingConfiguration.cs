using Jaeger;
using Jaeger.Senders;
using Jaeger.Senders.Thrift;
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
                var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
                var configuration = serviceProvider.GetRequiredService<IConfiguration>();

                Configuration.SenderConfiguration.DefaultSenderResolver = new SenderResolver(loggerFactory)
	                .RegisterSenderFactory<ThriftSenderFactory>();

                var tracer =  Configuration.FromIConfiguration(loggerFactory, configuration).GetTracer();

                GlobalTracer.Register(tracer);

                return tracer;
            });

            services.AddOpenTracing();

            return services;
        }
    }
}