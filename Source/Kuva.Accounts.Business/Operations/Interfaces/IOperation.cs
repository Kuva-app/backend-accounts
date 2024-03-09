using Kuva.Accounts.Entities;
using System.Threading.Tasks;

namespace Kuva.Accounts.Business.Operations.Interfaces
{
    interface IOperation
    {
        Task<UserEntity> Register(UserEntity newUser);

        Task<bool> Unregister(UserEntity user);
    }
}
