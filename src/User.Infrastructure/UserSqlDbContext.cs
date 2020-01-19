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
                optionsBuilder
                    .UseSqlServer("Server=tcp:127.0.0.1,1433;Initial Catalog=BaozDB;Persist Security Info=False;User ID=sa;Password=A4eneXa7@:<_;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=True;Connection Timeout=30;",
                    option =>
                    {
                        option.MigrationsHistoryTable("EFMigrationsHistory");
                        option.EnableRetryOnFailure(maxRetryCount: 5,
                            maxRetryDelay: TimeSpan.FromSeconds(30),
                            errorNumbersToAdd: null);
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
