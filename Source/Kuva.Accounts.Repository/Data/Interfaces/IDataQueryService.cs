using System.Collections.Generic;
using System.Threading.Tasks;

namespace Kuva.Accounts.Repository.Data.Interfaces
{
    public interface IDataQueryService<T>
        where T : class
    {
        Task<T> GetDataByPrimaryKeyAsync<TPk>(TPk primaryKey);
        Task<IEnumerable<T>> GetDataAsync(int page, int take);
    }
}