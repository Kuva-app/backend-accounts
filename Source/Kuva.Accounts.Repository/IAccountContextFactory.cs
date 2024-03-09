using System.Threading.Tasks;
using System;

namespace Kuva.Accounts.Repository
{
    public interface IAccountContextFactory
    {
        public AccountsContext DbContext { get; set; }

        Task<bool> SaveAsync(Action onSaveChangesAction = null);

        public T Map<T>(object source);
    }
}
