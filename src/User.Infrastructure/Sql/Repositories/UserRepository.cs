using Baoz.Infrastructure.SqlServer.Contracts;
using Baoz.Infrastructure.SqlServer.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using User.Core.Domain.Database;
using User.Core.Domain.Repositories;

namespace User.Infrastructure.Sql.Repositories
{
    public class UserRepository : EFRepository<Core.Domain.Entities.UserView>, IUserRepository
    {
        public UserRepository(IUserSqlDbContext dbContext) : base(dbContext)
        {
        }
    }
}
