using System.Threading.Tasks;

namespace Kuva.Accounts.Repository.Data.Interfaces
{
    public interface IDataModificationService<T>
        where T : class
    {
        Task<T> AddDataAsync(T data);
        Task<bool> UpdateDataAsync(T data);
        Task<bool> DeleteDataByPkAsync<TPk>(TPk id);
    }
}
