using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Kuva.Accounts.Entities;
using Kuva.Accounts.Repository.Data.Interfaces;
using Kuva.Accounts.Repository.Domain;
using Microsoft.EntityFrameworkCore;

#nullable enable
namespace Kuva.Accounts.Repository.Data
{
    public sealed class UserTokenData : BaseData, IUserTokenData
    {
        public UserTokenData(IDbContextFactory<AccountsContext> dbContext, IMapper mapper) : base(dbContext, mapper)
        {
        }

        public async Task<UserTokenEntity?> GetDataByPrimaryKeyAsync<TPk>(TPk primaryKey)
        {
            if (!(primaryKey is string id))
                return default;
            return Map<UserTokenEntity>(await DbContext.UserToken.FirstOrDefaultAsync(_ => _.Id == id));
        }

        public async Task<IEnumerable<UserTokenEntity>> GetDataAsync(int page, int take)
        {
            var userTokens = await DbContext.UserToken.Skip((page - 1) * take).Take(take)
                .ToListAsync();
            return Map<List<UserTokenEntity>>(userTokens);
        }

        public async Task<UserTokenEntity> AddDataAsync(UserTokenEntity data)
        {
            var newUserToken = Map<UserTokenDomain>(data);
            
            if (string.IsNullOrEmpty(newUserToken.Id) || string.IsNullOrWhiteSpace(newUserToken.Id))
                newUserToken.Id = Guid.NewGuid().ToString();
            if (newUserToken.CreateAt == default)
                newUserToken.CreateAt = DateTime.Now;
            
            await DbContext.UserToken.AddAsync(newUserToken);
            await SaveAsync();
            return Map<UserTokenEntity>(newUserToken);
        }

        public async Task<bool> UpdateDataAsync(UserTokenEntity data)
        {
            var selectedUserToken = await DbContext.UserToken.FirstOrDefaultAsync(_ => _.Id == data.Id);
            if (selectedUserToken == null)
                return false;
            selectedUserToken.ConfirmationCode = data.ConfirmationCode;
            DbContext.UserToken.Update(selectedUserToken);
            return await SaveAsync();
        }

        public async Task<bool> DeleteDataByPkAsync<TPrimaryKey>(TPrimaryKey id)
        {
            if (!(id is string tokenId))
                return false;
            var selectedUserToken = await DbContext.UserToken.FirstOrDefaultAsync(_ => _.Id == tokenId);
            if (selectedUserToken == null)
                return false;
            DbContext.UserToken.Remove(selectedUserToken);
            return await SaveAsync();
        }

        public async Task<UserTokenEntity?> GetUserTokenByUserIdAsync(long userId)
        {
            return Map<UserTokenEntity>(await DbContext.UserToken.FirstOrDefaultAsync(_ => _.UserId == userId));
        }
    }
}