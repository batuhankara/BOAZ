using System;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Baoz.Infrastructure.EventStore;
using BAOZ.Api.Configurations;
using BAOZ.Api.Filters;
using BAOZ.Api.Modules;
using BAOZ.Api.Sentry;
using BAOZ.Api.ValidationModules;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Sentry.Extensibility;
using User.Core.Domain.Commands;
using User.Infrastructure;

namespace BAOZ.Api
{
    public class Startup
    {

        public IConfigurationRoot Configuration { get; private set; }

        public ILifetimeScope AutofacContainer { get; private set; }

        public Startup(IWebHostEnvironment env)
        {
            var builder = new ConfigurationBuilder()
      .SetBasePath(env.ContentRootPath)
      .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
      .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
      .AddEnvironmentVariables();
            this.Configuration = builder.Build();
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<ISentryEventProcessor, EventProcessor>();

            services.AddControllers(options =>
            {
                options.Filters.Add(new ModelStateFilter());
            })
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0)
               .AddFluentValidation()
                .AddNewtonsoftJson(x => x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
            });
            services.AddWebApiValidations();
            services.AddOptions();
            services.AddLogging();

        }
        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.AddEventSourcing(Configuration);
            builder.RegisterType<EventStoreStreamNameFactory>().As<IEventStoreStreamNameFactory>().SingleInstance();
            builder.RegisterModule(new WebIocModule());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            this.AutofacContainer = app.ApplicationServices.GetAutofacRoot();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseSwagger()
                .UseEventSourcing();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
