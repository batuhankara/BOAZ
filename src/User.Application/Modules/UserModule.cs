using Autofac;
using Baoz.Infrastructure.Settings;
using Baoz.Infrastructure.SqlServer.Contracts;
using Baoz.Infrastructure.SqlServer.Repositories;
using BAOZ.Common.Helpers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using User.Core.Domain.Database;
using User.Core.Domain.Repositories;
using User.Infrastructure;
using User.Infrastructure.Sql;
using User.Infrastructure.Sql.Repositories;

namespace User.Application.Modules
{
    public class UserModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            #region DbContext Registrations
            builder.Register(componentContext =>
            {
                var env = componentContext.Resolve<IWebHostEnvironment>();
                bool isDevelopment = env.EnvironmentName == "Development";
                var configuration = componentContext.Resolve<IConfiguration>();

                var optionsBuilder = new DbContextOptionsBuilder<UserSqlDbContext>()
                    .UseNpgsql(configuration.GetReadStoreConnectionString(), option =>
                    {
                        option.MigrationsHistoryTable("EFMigrationsHistory");
                    });

                optionsBuilder.EnableSensitiveDataLogging(!isDevelopment);
                optionsBuilder.EnableDetailedErrors(isDevelopment);

                var context = new UserSqlDbContext(optionsBuilder.Options);
                context.Database.Migrate();
                return context;
            }).AsSelf()
              .AsImplementedInterfaces()
              .InstancePerLifetimeScope();
            #endregion
            builder.RegisterType<PasswordService>().As<IPasswordService>().SingleInstance();
            builder.RegisterType<UserRepository>().As<IUserRepository>().InstancePerLifetimeScope();
            builder.RegisterType<UnitOfWork<IUserSqlDbContext>>().As<IUnitOfWork<IUserSqlDbContext>>().InstancePerLifetimeScope();
            builder.RegisterType<UserDatabaseInitializer>().As<IUserDatabaseInitializer>().InstancePerDependency();

        }
    }
}
