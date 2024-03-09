using System.Threading.Tasks;

#nullable enable
namespace Kuva.Accounts.Business.Interfaces
{
    public interface IChangePasswordBusiness
    {
        Task<bool> ChangePasswordByUserIdAsync(long userId, string confirmationCode, string newPassword);
    }
}
