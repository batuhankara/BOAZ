using System.IO;
using Microsoft.AspNetCore.Hosting;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Sentry.Extensions.Logging;
using System;
using Microsoft.Extensions.Logging;
using Sentry;
using Sentry.Protocol;
using System.Collections.Generic;

namespace BAOZ.Api
{

    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args);
            host.Build().Run();
        }

        private static IHostBuilder CreateHostBuilder(string[] args)
        {


            var host = Host
                .CreateDefaultBuilder(args)
                    .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                    .ConfigureWebHostDefaults(webBuilder =>
                    {
                        webBuilder.ConfigureServices(services => services.AddAutofac())
                        .UseSentry(options =>
                        {
                            options.BeforeSend = @event =>
                        {
                            // Never report server names
                            @event.ServerName = null;
                            return @event;
                        };
                        })
                        .ConfigureKestrel((context, options) =>
                        {
                            options.AllowSynchronousIO = true;

                        })

                        .UseContentRoot(Directory.GetCurrentDirectory())
                        .UseStartup<Startup>();
                    }).ConfigureLogging((c, l) =>
                    {
                        l.AddConfiguration(c.Configuration);
                        l.AddConsole();
                        // Adding Sentry integration to Microsoft.Extensions.Logging
                        l.AddSentry();
                    });
            return host;
        }
    }
}
