using System;
using Utilities.Security.Cryptography;
using Utilities.Security.Utils;
using Kuva.Accounts.Business.UseCases.Interfaces;
using Kuva.Accounts.Entities;
using Newtonsoft.Json;

#nullable enable
namespace Kuva.Accounts.Business.UseCases
{
    internal class RequestChangePasswordUseCase : IRequestChangePasswordUseCase
    {
        public UserTokenEntity GetNewTokenBy(long userId)
        {
            var newConfirmationCode = StringUtils.GenerateRandomCode(Constants.ConfirmationCodeMaxLenght,
                Constants.ConfirmationCodeChars);
            
            var newToken = new UserTokenEntity
            {
                Id = Guid.NewGuid().ToString(),
                ConfirmationCode = newConfirmationCode,
                CreateAt = DateTime.Now,
                UserId = userId
            };
            
            return newToken;
        }

        public string GetHashByToken(UserTokenEntity userToken)
        {
            var jsonBody = JsonConvert.SerializeObject(userToken);
            var hash = CryptographyUtil.Shared.EncryptRijndael(jsonBody, Constants.HashSecurityKey);
            return hash;
        }

        public UserTokenEntity? GetUserTokenByHash(string hash)
        {
            var jsonBody = CryptographyUtil.Shared.DecryptRijndael(hash, Constants.HashSecurityKey);
            if (string.IsNullOrEmpty(jsonBody) || string.IsNullOrWhiteSpace(jsonBody))
                return default;
            try
            {
                return JsonConvert.DeserializeObject<UserTokenEntity>(jsonBody);
            }
            catch
            {
                return default;
            }
        }

        public bool Expired(DateTime createAt)
        {
            var expireOn = createAt.AddHours(Constants.RequestChangePasswordExpireHours);
            return DateTime.Now > expireOn;
        }
    }
}