using Baoz.Infrastructure.SqlServer.Contracts;
using User.Core.Domain.Entities;

namespace User.Core.Domain.Repositories
{
    public interface IUserRepository : IEFRepository<UserView>
    {
    }
}
