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
using Cart.Service.API.Infrastructure.AutoMapper;
using Cart.Service.API.Infrastructure.MongoDB;
using AutoMapper;
using Cart.Service.API.Repositories;
using Cart.Service.API.Services;
using Refit;
using Cart.Service.API.Infrastructure.AmazonSQS;
using Cart.Service.API.Infrastructure.Filters;
using FluentValidation.AspNetCore;
using Cart.Service.API.Validators;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Swagger;
using Cart.Service.API.Infrastructure.OpenTracing;

namespace cart.service.API
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
            services.AddControllers(options =>
            {
                options.Filters.Add<BusinessExceptionFilter>();
            })
            .AddFluentValidation(config =>
            {
                config.RegisterValidatorsFromAssemblyContaining<CreateCartRequestValidator>();
            });

            services.AddJaegerOpenTracing();

            services.AddHealthChecks();

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "Cart.Service API", Version = "v1" });
                options.CustomSchemaIds(x => x.FullName);
                options.AddFluentValidationRules();
            });

            services.AddAutoMapper(typeof(CartProfile).Assembly);

            services.AddMongoDB(Configuration);

            services.AddAmazonSQS(Configuration);

            services.AddScoped<ICartRepository, CartRepository>();

            services.AddScoped<IProductService>(serviceProvider =>
            {
                var productServiceURI = Configuration["ProductServiceURI"];
                return RestService.For<IProductService>(productServiceURI);
            });
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
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Cart.Service API V1");
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
