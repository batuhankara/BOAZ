using BAOZ.Api.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;


namespace BAOZ.Api.Configurations
{
    public static class DatabaseService
    {
        public static IApplicationBuilder UpdateDatabase(this IApplicationBuilder applicationBuilder)
        {
            using (var scope = applicationBuilder.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                scope
                    .ServiceProvider
                    .MigrateDatabase()
                    .Wait();

                scope
                    .ServiceProvider
                    .SeedDatabase()
                    .Wait();
            }

            return applicationBuilder;
        }
    }
}
