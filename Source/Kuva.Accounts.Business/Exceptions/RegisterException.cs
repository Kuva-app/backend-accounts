#nullable enable
namespace Kuva.Accounts.Business.Exceptions
{
    public class RegisterException : AccountsBusinessException
    {
        public RegisterException(AccountsErrors accountsErrors, string? paramName = null) : base(accountsErrors)
        {
            ParamName = paramName;
        }

        public string? ParamName { get; }
    }
}
