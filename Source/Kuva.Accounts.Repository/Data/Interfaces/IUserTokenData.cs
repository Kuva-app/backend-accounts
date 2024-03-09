using System.Threading.Tasks;
using Kuva.Accounts.Entities;

namespace Kuva.Accounts.Repository.Data.Interfaces
{
    public interface IUserTokenData: IDataQueryService<UserTokenEntity>, IDataModificationService<UserTokenEntity>
    {
        Task<UserTokenEntity> GetUserTokenByUserIdAsync(long userId);
    }
}