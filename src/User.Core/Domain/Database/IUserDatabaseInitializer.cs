using Baoz.Infrastructure.SqlServer.Contracts;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace User.Core.Domain.Database
{
    public interface IUserDatabaseInitializer: IDatabaseInitializer
    {
        Task DropDatabaseAsync();

    }
}
