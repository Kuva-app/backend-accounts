using System.Threading.Tasks;
using Kuva.Accounts.Entities;

namespace Kuva.Accounts.Business.Interfaces
{
    public interface IRequestChangePasswordBusiness
    {
        Task<string> RequestByUserIdAsync(long userId);
        Task<string> RequestByUserEmailAsync(string email);
        Task<bool> HasTokenRegisteredAsync(long userId);
        Task<UserEntity> GetUserByTokenHashAsync(string hash);
    }
}