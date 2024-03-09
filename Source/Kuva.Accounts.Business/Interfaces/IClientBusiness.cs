using Kuva.Accounts.Entities;
using System.Threading.Tasks;

namespace Kuva.Accounts.Business.Interfaces
{
    public interface IClientBusiness
    {
        Task<UserEntity> Register(UserEntity newClient, string password);

        Task<bool> Unregister(long userId);
    }
}
