using Kuva.Accounts.Business.Interfaces;
using Kuva.Accounts.Business.Operations;
using Kuva.Accounts.Business.UseCases.Interfaces;
using Kuva.Accounts.Entities;
using System.Threading.Tasks;
using Kuva.Accounts.Business.Exceptions;

#nullable enable
namespace Kuva.Accounts.Business
{
    public class ClientBusiness : IClientBusiness
    {
        public ClientBusiness(IClientUseCase clientUseCase, 
                              ClientOperations clientOperations,
                              ISearchUserBusiness searchUserBusiness)
        {
            _clientUseCase = clientUseCase;
            _clientOperations = clientOperations;
            _searchUserBusiness = searchUserBusiness;
        }

        private readonly IClientUseCase _clientUseCase;
        private readonly ClientOperations _clientOperations;
        private readonly ISearchUserBusiness _searchUserBusiness;

        public async Task<UserEntity> Register(UserEntity newClient, string password)
        {
            
            if (await Exists(newClient.Email))
                throw new RegisterException(AccountsErrors.EmailAlreadyExists);
            
            newClient.Passcode = _clientUseCase.GetHashPasscode(password, newClient.Email);
            
            return await _clientOperations.Register(newClient);
        }

        public async Task<bool> Unregister(long userId)
        {
            var current = await _searchUserBusiness.SearchUserByIdAsync(userId);
            
            if (current == null)
                throw new UnregisterException(AccountsErrors.UserNotFound);
            
            return await _clientOperations.Unregister(current);
        }

        private async Task<bool> Exists(string email)
        {
            var currentUser = await _searchUserBusiness.SearchUserByEmailAsync(email);
            return currentUser != null;
        }
    }
}
