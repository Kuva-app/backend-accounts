using System;
using System.Diagnostics;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Kuva.Accounts.Repository.Data
{
    public abstract class BaseData
    {
        protected readonly AccountsContext DbContext;
        private readonly IMapper _mapper;

        protected BaseData(IDbContextFactory<AccountsContext> dbContext, IMapper mapper)
        {
            DbContext = dbContext.CreateDbContext();
            _mapper = mapper;
        }
        
        /// <summary>
        /// Save changes
        /// </summary>
        /// <param name="onSaveChangesAction">Run action after save changes.</param>
        /// <exception cref="Exception">Return exception</exception>
        /// <returns></returns>
        protected async Task<bool> SaveAsync(Action onSaveChangesAction = null)
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
        
        protected T Map<T>(object source)
        {
            return source == null ? default : _mapper.Map<T>(source);
        }
    }
}