using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Kuva.Accounts.Entities;
using Kuva.Accounts.Repository.Data.Interfaces;
using Kuva.Accounts.Repository.Domain;
using Microsoft.EntityFrameworkCore;

namespace Kuva.Accounts.Repository.Data
{
    public class UserLevelData: BaseData, IUserLevelData
    {
        public UserLevelData(IDbContextFactory<AccountsContext> dbContext, IMapper mapper) : base(dbContext, mapper)
        {
        }
        
        public async Task<UserLevelEntity> GetDataByPrimaryKeyAsync<TPk>(TPk primaryKey)
        {
            if (!(primaryKey is short id))
                return default;
            var userLevel = await DbContext.UserLevel.FirstOrDefaultAsync(_ => _.Id == id);
            return Map<UserLevelEntity>(userLevel);
        }

        public async Task<IEnumerable<UserLevelEntity>> GetDataAsync(int page = 1, int take = 10)
        {
            var userLevels = await DbContext.UserLevel
                .Skip((page - 1) * take)
                .Take(take)
                .ToListAsync();
            return Map<List<UserLevelEntity>>(userLevels);
        }

        public async Task<UserLevelEntity> AddDataAsync(UserLevelEntity data)
        {
            var userLevel = Map<UserLevelDomain>(data);
            if (userLevel == null)
                return default;

            var valueEntry = await DbContext.UserLevel.AddAsync(userLevel);

            await SaveAsync();
            
            return valueEntry.State == EntityState.Added ? Map<UserLevelEntity>(valueEntry) : default;
        }

        public async Task<bool> UpdateDataAsync(UserLevelEntity data)
        {
            var userLevel = Map<UserLevelDomain>(data);
            if (userLevel == null)
                return false;

            var valueEntry = DbContext.UserLevel.Update(userLevel);

            await SaveAsync();

            return valueEntry.State == EntityState.Modified;
        }

        public async Task<bool> DeleteDataByPkAsync<TPrimaryKey>(TPrimaryKey id)
        {
            if (!(id is short userLevelId))
                return false;
            var selectedUserLevel = await DbContext.UserLevel.FirstOrDefaultAsync(_ => _.Id == userLevelId);
            if (selectedUserLevel == null)
                return false;
            DbContext.UserLevel.Remove(selectedUserLevel);
            return await SaveAsync();
        }
    }
}