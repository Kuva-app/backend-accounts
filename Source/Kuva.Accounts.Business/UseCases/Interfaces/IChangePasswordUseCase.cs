using Kuva.Accounts.Entities;

namespace Kuva.Accounts.Business.UseCases.Interfaces
{
    public interface IChangePasswordUseCase
    {
        string GeneratePasscode(UserEntity user, string newPassword);
    }
}
