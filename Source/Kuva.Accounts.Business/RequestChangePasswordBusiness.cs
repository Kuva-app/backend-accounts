using System.Threading.Tasks;
using Kuva.Accounts.Business.Exceptions;
using Kuva.Accounts.Business.Interfaces;
using Kuva.Accounts.Business.QueueConnector;
using Kuva.Accounts.Business.UseCases.Interfaces;
using Kuva.Accounts.Entities;
using Kuva.Accounts.Repository.Data.Interfaces;

#nullable enable
namespace Kuva.Accounts.Business
{
    public sealed class RequestChangePasswordBusiness : IRequestChangePasswordBusiness
    {
        public RequestChangePasswordBusiness(IUserTokenData userTokenData,
            IRequestChangePasswordUseCase useCase, IServiceQueueConnector serviceBus, 
            ISearchUserBusiness searchUserBusiness)
        {
            _userTokenData = userTokenData;
            _useCase = useCase;
            _serviceBus = serviceBus;
            _searchUserBusiness = searchUserBusiness;
        }
        
        private readonly IUserTokenData _userTokenData;
        private readonly IRequestChangePasswordUseCase _useCase;
        private readonly IServiceQueueConnector _serviceBus;
        private readonly ISearchUserBusiness _searchUserBusiness;

        public async Task<string?> RequestByUserIdAsync(long userId)
        {
            var selectedUser = await _searchUserBusiness.SearchUserByIdAsync(userId);
            if (selectedUser == null)
                throw new RequestChangePasswordException(AccountsErrors.UserNotFound);
            
            return await RequestByUserAsync(selectedUser);
        }

        public async Task<string> RequestByUserEmailAsync(string email)
        {
            var selectedUser = await _searchUserBusiness.SearchUserByEmailAsync(email);
            if (selectedUser == null)
                throw new RequestChangePasswordException(AccountsErrors.UserNotFound);
            return await RequestByUserAsync(selectedUser);
        }

        private async Task<string> RequestByUserAsync(UserEntity selectedUser)
        {
            if (await HasTokenRegisteredAsync(selectedUser.Id))
                throw new RequestChangePasswordException(AccountsErrors.RequestChangePasswordTokenAlreadyRegistered);

            var newToken = _useCase.GetNewTokenBy(selectedUser.Id);
            newToken = await _userTokenData.AddDataAsync(newToken);

            if (newToken == null)
                throw new RequestChangePasswordException(AccountsErrors.RequestChangePasswordTokenNotRegistered);

            await _serviceBus.SendMessageToQueueAsync(Constants.RequestChangePasswordQueueName, newToken);

            return _useCase.GetHashByToken(newToken);
        }

        public async Task<bool> HasTokenRegisteredAsync(long userId)
        {
            var token = await _userTokenData.GetUserTokenByUserIdAsync(userId);
            return token != null;
        }

        public async Task<UserEntity> GetUserByTokenHashAsync(string hash)
        {
            var sentUserToken = _useCase.GetUserTokenByHash(hash);
            var userToken = await _userTokenData.GetDataByPrimaryKeyAsync(sentUserToken.Id);

            if (userToken == null)
                throw new RequestChangePasswordException(AccountsErrors.RequestChangePasswordTokenNotRegistered);
            
            if (!userToken.Id.Equals(sentUserToken.Id))
                throw new RequestChangePasswordException(AccountsErrors.RequestChangePasswordTokenNotRegistered);

            if (_useCase.Expired(userToken.CreateAt))
                throw new RequestChangePasswordException(AccountsErrors.RequestChangePasswordTokenExpired);

            var user = await _searchUserBusiness.SearchUserByIdAsync(userToken.UserId);
            if (user == null)
                throw new RequestChangePasswordException(AccountsErrors.UserNotFound);
            
            return user;
        }
    }
}