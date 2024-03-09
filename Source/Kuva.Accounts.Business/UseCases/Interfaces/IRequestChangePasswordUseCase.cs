using System;
using Kuva.Accounts.Entities;

namespace Kuva.Accounts.Business.UseCases.Interfaces
{
    public interface IRequestChangePasswordUseCase
    {
        UserTokenEntity GetNewTokenBy(long userId);
        string GetHashByToken(UserTokenEntity userToken);
        UserTokenEntity GetUserTokenByHash(string hash);
        bool Expired(DateTime createAt);
    }
}