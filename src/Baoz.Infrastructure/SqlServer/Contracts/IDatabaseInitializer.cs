using System.Threading.Tasks;

namespace Baoz.Infrastructure.SqlServer.Contracts
{
    public interface IDatabaseInitializer
    {
        Task MigrateAsync();
        Task SeedAsync();
    }
}
