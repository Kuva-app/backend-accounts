namespace Kuva.Accounts.Business.Exceptions
{
    public class SearchUserException<T> : AccountsBusinessException
    {
        public SearchUserException(AccountsErrors accountsErrors, T data) : base(accountsErrors)
        {
            this._data = data;
        }

        private readonly T _data;

        public T GetData()
        {
            return _data;
        }
    }
}