using System;
using Kuva.Accounts.Entities;
using Utilities.Security.Cryptography;
using Kuva.Accounts.Business.UseCases.Interfaces;

namespace Kuva.Accounts.Business.UseCases
{
    internal class ChangePasswordUseCase : IChangePasswordUseCase
    {
        public string GeneratePasscode(UserEntity user, string newPassword)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            if (string.IsNullOrEmpty(newPassword))
                throw new ArgumentNullException(nameof(newPassword));

            return CryptographyUtil.Shared.EncryptSha256($"{Constants.UserPasswordSalt}{newPassword}{user.Email}");
        }
    }
}