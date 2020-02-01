using System;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Baoz.Infrastructure.EventStore;
using BAOZ.Api.Configurations;
using BAOZ.Api.Filters;
using BAOZ.Api.Modules;
using BAOZ.Api.ValidationModules;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using User.Core.Domain.Commands;
using User.Infrastructure;

namespace BAOZ.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {

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
            var containerBuilder = new ContainerBuilder();
            containerBuilder.AddEventSourcing(Configuration);
            containerBuilder.RegisterType<EventStoreStreamNameFactory>().As<IEventStoreStreamNameFactory>().SingleInstance();
            containerBuilder.RegisterModule(new WebIocModule());
            containerBuilder.Populate(services);

            var container = containerBuilder.Build();
            //DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            return new AutofacServiceProvider(container);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            using (var scope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                scope.ServiceProvider.GetService<UserSqlDbContext>().Database.EnsureCreated();
            }
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

            app.UseHttpsRedirection()
                .UpdateDatabase();

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
