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
using AutoMapper;
using Product.Service.API.Infrastructure.AutoMapper;
using Product.Service.API.Infrastructure.MongoDB;
using Product.Service.API.Repositories;
using FluentValidation.AspNetCore;
using Product.Service.API.Validators;
using Product.Service.API.Infrastructure.Filters;
using Microsoft.OpenApi.Models;
using Product.Service.API.Infrastructure.OpenTracing;

namespace product.service.API
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
                config.RegisterValidatorsFromAssemblyContaining<CreateProductRequestValidator>();
            });            

            services.AddJaegerOpenTracing();

            services.AddHealthChecks();

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "Product.Service API", Version = "v1" });
                options.CustomSchemaIds(x => x.FullName);
            });

            services.AddAutoMapper(typeof(ProductProfile).Assembly);

            services.AddMongoDB(Configuration);

            services.AddScoped<IProductRepository, ProductRepository>();
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
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Product.Service API V1");
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
