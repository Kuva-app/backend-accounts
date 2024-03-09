using System.Threading.Tasks;
using Kuva.Accounts.Entities;

#nullable enable
namespace Kuva.Accounts.Business.Interfaces
{
    public interface ISearchUserBusiness
    {
        Task<UserEntity?> SearchUserByIdAsync(long id);

        Task<UserEntity?> SearchUserByEmailAsync(string email);
    }
}
