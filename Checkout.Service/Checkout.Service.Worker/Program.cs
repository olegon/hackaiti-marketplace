using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Checkout.Service.Worker.Infrastructure.AmazonSQS;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Prometheus;
using Serilog;

namespace Checkout.Service.Worker
{
    [ExcludeFromCodeCoverage]
    public class Program
    {
        public static int Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables()
                .Build();

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();

            try
            {
                ConfigurePrometheusListener(configuration);

                Log.Information("Starting worker host");
                CreateHostBuilder(args).Build().Run();
                return 0;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");
                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        private static void ConfigurePrometheusListener(IConfigurationRoot configuration)
        {
            var port = int.Parse(configuration["PrometheusPort"]);
            Log.Information("Starting Prometheus listener at {uri}", $"localhost:{port}");
            var server = new MetricServer(hostname: "localhost", port: port);
            server.Start();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddAmazonSQS(hostContext.Configuration);

                    services.AddHostedService<StartCheckoutWorker>();
                })
                .UseSerilog();
    }
}
