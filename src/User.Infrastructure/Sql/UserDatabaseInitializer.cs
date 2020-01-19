using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using User.Core.Domain.Database;

namespace User.Infrastructure.Sql
{
   public class UserDatabaseInitializer : IUserDatabaseInitializer
    {
        private readonly UserSqlDbContext _dbContext;

        public UserDatabaseInitializer(UserSqlDbContext masterDbContext)
        {
            _dbContext = masterDbContext;
        }

        public async Task DropDatabaseAsync()
        {
            await Task.CompletedTask;
        }

        public async Task MigrateAsync()
        {
            await _dbContext.Database.MigrateAsync();
        }

        public Task SeedAsync()
        {
            return Task.CompletedTask;
        }
    }
}