using System;
using System.Threading.Tasks;
using Kuva.Accounts.Business.Exceptions;
using Kuva.Accounts.Business.Interfaces;
using Kuva.Accounts.Business.Validators;
using Kuva.Accounts.Entities;
using Kuva.Accounts.Repository.Data.Interfaces;
using Microsoft.AspNetCore.Mvc;

#nullable enable
namespace Kuva.Accounts.Business
{
    public class SearchUserBusiness: ISearchUserBusiness
    {
        public SearchUserBusiness([FromServices] IUserData userData)
        {
            _userData = userData;
        }
        
        private readonly IUserData _userData;
        
        public async Task<UserEntity?> SearchUserByIdAsync(long id)
        {
            return await _userData.GetDataByPrimaryKeyAsync(id);
        }

        public async Task<UserEntity?> SearchUserByEmailAsync(string email)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrWhiteSpace(email))
                throw new ArgumentNullException(nameof(email));

            if (!EmailValidator.Shared.IsTrue(email))
                throw new SearchUserException<string>(AccountsErrors.InvalidUserEmail, email);

            return await _userData.GetUserByEmailAsync(email);
        }
    }
}
