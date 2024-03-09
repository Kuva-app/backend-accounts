using System.Collections.Generic;
using System.Threading.Tasks;
using Kuva.Accounts.Entities;

namespace Kuva.Accounts.Repository.Data.Interfaces
{
    public interface IUserData : IDataQueryService<UserEntity>, IDataModificationService<UserEntity>
    {
        Task<UserEntity> GetUserByEmailAsync(string email);
        Task<IEnumerable<UserEntity>> GetAllActivesAsync(int page, int take);
        Task<IEnumerable<UserEntity>> GetAllInactivesAsync(int page, int take);
        Task<bool> ChangePasswordByIdAsync(long id, string newPasscode);
    }
}