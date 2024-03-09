namespace Kuva.Accounts.Business.Exceptions
{
    public class UnregisterException : AccountsBusinessException
    {
        public UnregisterException(AccountsErrors accountsErrors) : base(accountsErrors)
        {
        }
    }
}
