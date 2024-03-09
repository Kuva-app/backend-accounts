using Kuva.Accounts.Business.UseCases.Interfaces;
using Bitx.Security.Cryptography;

namespace Kuva.Accounts.Business.UseCases
{
    public class ClientUseCase : IClientUseCase
    {
        public string GetHashPasscode(string password, string email)
        {
            return CryptographyUtil.Shared.EncryptSha256($"{Constants.UserPasswordSalt}{password}{email}");
        }
    }
}
