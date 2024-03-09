using System;
using Kuva.Accounts.Business.Operations.Interfaces;
using Kuva.Accounts.Entities;
using System.Threading.Tasks;
using Kuva.Accounts.Repository.Data.Interfaces;
using Microsoft.AspNetCore.Mvc;

#nullable enable
namespace Kuva.Accounts.Business.Operations
{
    public abstract class BaseOperations : IOperation
    {
        protected BaseOperations([FromServices] IUserData userData, UserLevels userLevel)
        {
            _userData = userData;
            _userLevel = userLevel;
        }

        private readonly IUserData _userData;
        private readonly UserLevels _userLevel;

        public virtual async Task<UserEntity> Register(UserEntity newUser)
        {
            RegisterValidate(newUser);
            newUser.UserLevelId = _userLevel;
            newUser.CreateAt = DateTime.Now;
            newUser.Active = false;
            return await _userData.AddDataAsync(newUser);
        }

        public virtual async Task<bool> Unregister(UserEntity user)
        {
            UnregisterValidate(user);
            return await _userData.DeleteDataByPkAsync(user.Id);
        }

        protected abstract void RegisterValidate(UserEntity newUser);
        protected abstract void UnregisterValidate(UserEntity user);
    }
}
