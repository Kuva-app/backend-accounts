using System.Threading.Tasks;
using Kuva.Accounts.Business.Exceptions;
using Kuva.Accounts.Business.Interfaces;
using Kuva.Accounts.Business.UseCases.Interfaces;
using Kuva.Accounts.Repository.Data.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Kuva.Accounts.Business
{
    public class ChangePasswordBusiness: IChangePasswordBusiness
    {
        private readonly IUserData _userData;
        private readonly IUserTokenData _userTokenData;
        private readonly IChangePasswordUseCase _useCase;

        public ChangePasswordBusiness([FromServices] IUserData userData,
            [FromServices] IUserTokenData userTokenData,
            [FromServices] IChangePasswordUseCase useCase)
        {
            _userData = userData;
            _userTokenData = userTokenData;
            _useCase = useCase;
        }
        
        public async Task<bool> ChangePasswordByUserIdAsync(long userId, string confirmationCode, string newPassword)
        {
            if (userId <= 0)
                throw new ChangePasswordException(userId, AccountsErrors.InvalidUserId);

            if (string.IsNullOrEmpty(newPassword) || string.IsNullOrWhiteSpace(newPassword))
                throw new ChangePasswordException(userId, AccountsErrors.PasswordNullOrEmpty);
            
            var selectedUser = await _userData.GetDataByPrimaryKeyAsync(userId);
            if (selectedUser == null)
                throw new ChangePasswordException(userId, AccountsErrors.UserNotFound);
            
            var token = await _userTokenData.GetUserTokenByUserIdAsync(userId);
            if (token == null || !token.ConfirmationCode.Equals(confirmationCode))
                throw new ChangePasswordException(userId, AccountsErrors.RequestChangePasswordTokenNotRegistered);
            
            var newPasscode = _useCase.GeneratePasscode(selectedUser, newPassword);
            if (newPasscode.Length > 80)
                throw new ChangePasswordException(userId, AccountsErrors.PasswordToBig);
            
            return await _userData.ChangePasswordByIdAsync(selectedUser.Id, newPasscode);
        }

    }
}
