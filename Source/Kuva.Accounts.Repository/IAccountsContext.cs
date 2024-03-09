using Kuva.Accounts.Repository.Domain;
using Microsoft.EntityFrameworkCore;

namespace Kuva.Accounts.Repository
{
    public interface IAccountsContext
    {
        DbSet<UserDomain> User { get; }
        DbSet<UserLevelDomain> UserLevel { get; }
        DbSet<UserTokenDomain> UserToken { get; }
    }
}