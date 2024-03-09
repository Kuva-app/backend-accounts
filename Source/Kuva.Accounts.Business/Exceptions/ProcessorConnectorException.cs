namespace Kuva.Accounts.Business.Exceptions
{
    public class ProcessorConnectorException : AccountsBusinessException
    {
        public ProcessorConnectorException(AccountsErrors accountsErrors) : base(accountsErrors)
        {
        }
    }
}