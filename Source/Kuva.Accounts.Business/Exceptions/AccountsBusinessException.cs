using System;

namespace Kuva.Accounts.Business.Exceptions
{
    public class AccountsBusinessException : Exception
    {
        public AccountsBusinessException(AccountsErrors accountsErrors) : base(accountsErrors.GetAccountsErrors().Value)
        {
            AccountsError = accountsErrors;
            ErrorCode = accountsErrors.GetAccountsErrors().Key;
        }
        public AccountsErrors AccountsError { get; }
        public string ErrorCode { get; }
    }
}