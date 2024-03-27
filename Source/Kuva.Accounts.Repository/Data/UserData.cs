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
    public class UserData : BaseData, IUserData
    {
        public UserData(IDbContextFactory<AccountsContext> dbContext, IMapper mapper) : base(dbContext, mapper)
        {
        }

        public async Task<UserEntity> GetUserByEmailAsync(string email)
        {
            var user = await DbContext.User.FirstOrDefaultAsync(_ => _.Email == email);
            return Map<UserEntity>(user);
        }

        public async Task<UserEntity> GetDataByPrimaryKeyAsync<TPk>(TPk primaryKey)
        {
            if (!(primaryKey is long id))
                return default;
            var user = await DbContext.User.FirstOrDefaultAsync(_ => _.Id == id);
            return Map<UserEntity>(user);
        }
        
        public async Task<IEnumerable<UserEntity>> GetAllActivesAsync(int page, int take)
        {
            return await GetAllByStatusAsync(true, page, take);
        }

        public async Task<IEnumerable<UserEntity>> GetAllInactivesAsync(int page, int take)
        {
            return await GetAllByStatusAsync(false, page, take);
        }

        public async Task<bool> ChangePasswordByIdAsync(long id, string newPasscode)
        {
            var selectedUser = await DbContext.User.FirstOrDefaultAsync(_ => _.Id == id);
            if (selectedUser == null)
                return false;
            selectedUser.Passcode = newPasscode;
            DbContext.User.Update(selectedUser);
            return await SaveAsync();
        }

        public async Task<IEnumerable<UserEntity>> GetDataAsync(int page = 1, int take = 10)
        {
            var users = await DbContext.User
                .Skip((page - 1) * take)
                .Take(take)
                .ToListAsync();
            return Map<List<UserEntity>>(users);
        }

        public async Task<UserEntity> AddDataAsync(UserEntity data)
        {
            var user = Map<UserDomain>(data);
            if (user == null)
                return default;
            var added = await DbContext.User.AddAsync(user);
            await SaveAsync();
            return Map<UserEntity>(added.Entity);
        }

        public async Task<bool> UpdateDataAsync(UserEntity data)
        {
            var user = await DbContext.User.FirstOrDefaultAsync(_ => _.Id == data.Id);
            user.Active = data.Active;
            user.Name = data.Name;
            user.UserLevelId = (short) data.UserLevelId;
            DbContext.User.Update(user);
            var updated = await SaveAsync();
            return updated;
        }

        public async Task<bool> DeleteDataByPkAsync<TPk>(TPk id)
        {
            if (!(id is long userId))
                return false;
            var selectedUser = DbContext.User.FirstOrDefault(_ => _.Id == userId);
            if (selectedUser == null)
                return false;
            DbContext.User.Remove(selectedUser);
            return await SaveAsync();
        }

        private async Task<IEnumerable<UserEntity>> GetAllByStatusAsync(bool active, int page, int take)
        {
            var users = await DbContext.User.Where(_ => _.Active == active)
                .Skip((page - 1) * take)
                .Take(take)
                .ToListAsync();
            return Map<List<UserEntity>>(users);
        }
    }
}