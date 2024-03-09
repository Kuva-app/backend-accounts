using Kuva.Accounts.Business.Exceptions;
using Kuva.Accounts.Business.Validators;
using Kuva.Accounts.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using Kuva.Accounts.Repository.Data.Interfaces;

namespace Kuva.Accounts.Business.Operations
{
    public sealed class GuestOperations : BaseOperations
    {
        public GuestOperations([FromServices] IUserData userData) : base(userData, UserLevels.Guest)
        {
        }

        protected override void RegisterValidate(UserEntity newUser)
        {
            if (newUser == null)
                throw new ArgumentNullException(nameof(newUser));

            if (string.IsNullOrEmpty(newUser.Email) || string.IsNullOrWhiteSpace(newUser.Email))
                throw new NullReferenceException("E-mail é necessário.");

            if (!EmailValidator.Shared.IsTrue(newUser.Email))
                throw new RegisterException(AccountsErrors.InvalidUserEmail);

            if (string.IsNullOrEmpty(newUser.Name) || string.IsNullOrWhiteSpace(newUser.Name))
                throw new NullReferenceException("Nome é necessário");
        }

        protected override void UnregisterValidate(UserEntity user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));
        }
    }
}
