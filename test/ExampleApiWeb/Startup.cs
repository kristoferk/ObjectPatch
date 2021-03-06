﻿using AutoMapper;
using ExampleApiWeb.Api.v1.Contracts;
using ExampleApiWeb.Code.Repository;
using ExampleApiWeb.Framework;
using ExampleApiWeb.Framework.Repositories;
using ExampleApiWeb.Framework.Validation;
using ExampleApiWeb.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace ExampleApiWeb
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var configuration = new MapperConfiguration(cfg => {
                cfg.AllowNullCollections = true;
                cfg.ShouldMapProperty = p => p.GetMethod != null && p.GetMethod.IsPublic && p.Name != "ExtensionData";
                cfg.CreateMap<DateTime, DateTimeOffset>().ProjectUsing(f => DateToUtcOffset(f));
                cfg.CreateMap<DateTime, DateTimeOffset?>().ProjectUsing(f => DateToUtcOffset(f));
                cfg.CreateMap<DateTime?, DateTimeOffset>().ProjectUsing(f => f.HasValue ? DateToUtcOffset(f.Value) : DateTimeOffset.UtcNow);
                cfg.CreateMap<DateTime?, DateTimeOffset?>().ProjectUsing(f => f.HasValue ? (DateTimeOffset?)DateToUtcOffset(f.Value) : null);
                cfg.CreateMap<DateTimeOffset, DateTime>().ProjectUsing(f => f.UtcDateTime);
                cfg.CreateMap<DateTimeOffset, DateTime?>().ProjectUsing(f => f.UtcDateTime);
                cfg.CreateMap<DateTimeOffset?, DateTime?>().ProjectUsing(f => f.HasValue ? (DateTime?)f.Value.UtcDateTime : null);
                cfg.CreateMap<DateTimeOffset?, DateTime>().ProjectUsing(f => f.HasValue ? f.Value.UtcDateTime : DateTime.UtcNow);
                cfg.CreateMap<Customer, CustomerDto>().ReverseMap();
                cfg.CreateMap<ApiCollection<Customer>, ApiCollectionDto<CustomerDto>>().ReverseMap();
            });

            IMapper mapper = configuration.CreateMapper();

            // Add framework services.
            services.AddMvc();
            services.AddAuthorization();
            services.AddSingleton(mapper);

            services.AddScoped<IAuthorizationHandler, CustomAuthorizationHandler>();
            services.AddScoped<CustomerRepository, CustomerRepository>();
            services.AddScoped<Validator, Validator>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseMvc();
        }

        private static DateTimeOffset DateToUtcOffset(DateTime dateTime)
        {
            if (dateTime == DateTime.MinValue)
            {
                return DateTimeOffset.MinValue;
            }

            if (dateTime == DateTime.MaxValue)
            {
                return DateTimeOffset.MaxValue;
            }

            return DateTime.SpecifyKind(dateTime, DateTimeKind.Utc);
        }
    }
}