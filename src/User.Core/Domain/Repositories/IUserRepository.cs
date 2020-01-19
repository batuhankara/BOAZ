using Baoz.Infrastructure.SqlServer.Contracts;

namespace User.Core.Domain.Repositories
{
    public interface IUserRepository : IEFRepository<Entities.UserView>
    {
    }
}
