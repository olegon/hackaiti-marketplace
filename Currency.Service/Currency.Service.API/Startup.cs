using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Prometheus;
using Serilog;
using System.Diagnostics.CodeAnalysis;
using Currency.Service.API.Infrastructure.Redis;
using Currency.Service.API.Services;
using Refit;
using AutoMapper;
using Currency.Service.API.Infrastructure.AutoMapper;
using Microsoft.OpenApi.Models;
using Currency.Service.API.Infrastructure.OpenTracing;

namespace currency.service.API
{
    [ExcludeFromCodeCoverage]
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddJaegerOpenTracing();

            services.AddHealthChecks();

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "Currency.Service API", Version = "v1" });
                options.CustomSchemaIds(x => x.FullName);options.CustomSchemaIds(x => x.FullName);options.CustomSchemaIds(x => x.FullName);
            });

            services.AddAutoMapper(typeof(CurrencyProfile).Assembly);

            services.AddRedisClient(Configuration);

            services.AddScoped<IZupCurrencyService>(serviceProvider =>
            {
                return RestService.For<IZupCurrencyService>(Configuration["ZupCurrenciesServiceURI"]);
            });

            services.AddScoped<ICurrencyService, CurrencyService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // app.UseHttpsRedirection();

            app.UseSerilogRequestLogging();

            app.UseSwagger();

            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Currency.Service API V1");
            });

            app.UseRouting();

            app.UseHttpMetrics();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHealthChecks("/health");
                endpoints.MapMetrics();
                endpoints.MapControllers();
            });
        }
    }
}
