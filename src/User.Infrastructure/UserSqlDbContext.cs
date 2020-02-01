using Baoz.Infrastructure.SqlServer.Extensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using User.Core.Domain.Database;

namespace User.Infrastructure
{
    public class UserSqlDbContext : DbContext, IUserSqlDbContext
    {
        public DbContext ContextDatabase => this;

        public UserSqlDbContext(DbContextOptions<UserSqlDbContext> options) : base(options)
        {
        }

        public UserSqlDbContext()
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql("User ID=postgres;Password=example;Server=127.0.0.1; Port=5432;Database=boazdb",

                    option =>
                    {

                        option.MigrationsHistoryTable("EFMigrationsHistory");
                    });


                optionsBuilder.EnableSensitiveDataLogging(false);

                optionsBuilder.EnableDetailedErrors(true);
            }
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyAllTypeConfigurations<UserSqlDbContext>("User.Infrastructure.Sql.Mappings");
            builder.ModifyAllTypeConfigurations(); // ApplyAllTypeConfigurations'dan sonra gelmeli.            

            base.OnModelCreating(builder);
        }
        public override int SaveChanges()
        {
            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await base.SaveChangesAsync();
        }
    }

}
