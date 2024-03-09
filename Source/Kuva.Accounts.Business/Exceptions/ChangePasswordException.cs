namespace Kuva.Accounts.Business.Exceptions
{
    public class ChangePasswordException : AccountsBusinessException
    {
        public ChangePasswordException(long userId, AccountsErrors accountsErrors) : base(accountsErrors)
        {
            UserId = userId;
        }

        public long UserId { get; }
    }
}