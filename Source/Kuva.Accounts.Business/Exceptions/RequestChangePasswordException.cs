namespace Kuva.Accounts.Business.Exceptions
{
    public class RequestChangePasswordException : AccountsBusinessException
    {
        public RequestChangePasswordException(AccountsErrors accountsErrors) : base(accountsErrors)
        {
        }
    }
}