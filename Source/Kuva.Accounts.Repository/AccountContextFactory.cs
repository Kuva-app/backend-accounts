using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Threading.Tasks;
using System;

namespace Kuva.Accounts.Repository
{
    public sealed class AccountContextFactory : IAccountContextFactory
    {
        public AccountsContext DbContext { get; set; }

        private readonly IMapper _mapper;

        public AccountContextFactory(IDbContextFactory<AccountsContext> dbContextFactory, IMapper mapper)
        {
            DbContext = dbContextFactory.CreateDbContext();
            _mapper = mapper;
        }

        /// <summary>
        /// Save changes
        /// </summary>
        /// <param name="onSaveChangesAction">Run action after save changes.</param>
        /// <exception cref="Exception">Return exception</exception>
        /// <returns></returns>
        public async Task<bool> SaveAsync(Action onSaveChangesAction = null)
        {
            await DbContext.Database.OpenConnectionAsync();
            try
            {
                await DbContext.SaveChangesAsync();
                if (onSaveChangesAction != null)
                    await Task.Run(() => onSaveChangesAction);
                return true;
            }
            catch (Exception e)
            {
#if DEBUG
                Debug.WriteLine(":-> KUVA EF CORE ERROR: {0} - {1}", e.Message, e.StackTrace);
#endif
                return false;
            }
            finally
            {
                await DbContext.Database.CloseConnectionAsync();
            }
        }

        public T Map<T>(object source)
        {
            return source == null ? default : _mapper.Map<T>(source);
        }
    }
}
