using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using User.Core.Domain.Database;

namespace BAOZ.Api.Extensions
{
    public static class StartupConfigurationExtensions
    {
        public static async Task<IServiceProvider> MigrateDatabase(this IServiceProvider serviceProvider)
        {
            //TODO:
            //try
            //{
            var migrations = new[]
            {
                    serviceProvider.GetRequiredService<IUserDatabaseInitializer>()
                                   .MigrateAsync(),
                };

            await Task.WhenAll(migrations);
            //}
            //catch (Exception ex)
            //{
            //    var logger = serviceProvider.GetRequiredService<ILogger<Program>>();

            //    logger.LogCritical(LoggingEvents.InitDatabase, ex, LoggingEvents.InitDatabase.Name);
            //}

            return serviceProvider;
        }

        public static async Task<IServiceProvider> SeedDatabase(this IServiceProvider serviceProvider)
        {
            //TODO:
            //try
            //{
            var seedList = new[]
            {
                serviceProvider.GetRequiredService<IUserDatabaseInitializer>()
                               .SeedAsync(),
            };

            await Task.WhenAll(seedList);
            //}
            //catch (Exception ex)
            //{
            //    var logger = serviceProvider.GetRequiredService<ILogger<Program>>();

            //    logger.LogCritical(LoggingEvents.InitDatabase, ex, LoggingEvents.InitDatabase.Name);
            //}

            return serviceProvider;
        }
    }

}
