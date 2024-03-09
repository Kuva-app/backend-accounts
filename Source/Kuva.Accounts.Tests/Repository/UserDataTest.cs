using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Kuva.Accounts.Entities;
using Kuva.Accounts.Repository;
using Kuva.Accounts.Repository.Data;
using Kuva.Accounts.Repository.Data.Interfaces;
using Kuva.Accounts.Repository.Domain;
using Kuva.Accounts.Tests.Fixtures;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Moq;
using Utilities.General.XunitPriorityAttributes;

namespace Kuva.Accounts.Tests.Repository
{
    [TestCaseOrderer("Utilities.General.XunitPriorityAttributes.PriorityOrderer",
        "Utilities.General")]
    public class UserDataTest : BaseRepositoryTest
    {
        private readonly IUserData _sut;
        private readonly Mock<IAccountContextFactory> _dbContextFactoryMock = new();

        public UserDataTest(IMapper mapper)
        {
            _sut = new UserData(_dbContextFactoryMock.Object);
        }

        [Theory, TestPriority(0)]
        [MemberData(nameof(GetValidUsers))]
        public async Task InsertTest(UserEntity user)
        {
            _dbContextFactoryMock.Reset();
            _dbContextFactoryMock.Setup(_ => _.DbContext.User.AddAsync(It.IsAny<UserDomain>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((EntityEntry<UserDomain> entityEntry) =>
                {
                    return entityEntry;
                });

            var newUser = await _sut.AddDataAsync(user);

            Assert.NotNull(newUser);
            Assert.NotEqual(default, newUser.Id);
            Assert.Equal(user.Name, newUser.Name);
            Assert.Equal(user.Email, newUser.Email);
            Assert.Equal(user.Passcode, newUser.Passcode);
            Assert.Equal(user.UserLevelId, newUser.UserLevelId);
            Assert.Equal(user.Active, newUser.Active);
            Assert.Equal(user.CreateAt, newUser.CreateAt);
        }

        [Fact, TestPriority(1)]
        public async Task GetDataTest()
        {
            await _sut.AddDataAsync(UserFixture.GetUnregisteredUsers().First());

            var users = await _sut.GetDataAsync(1, 5);
            Assert.NotNull(users);
            var selectedUser = users.First();
            
            var user = await _sut.GetDataByPrimaryKeyAsync(selectedUser.Id);
            CompareSuccess(selectedUser, user);
            
            user = await _sut.GetUserByEmailAsync(selectedUser.Email);
            CompareSuccess(selectedUser, user);
        }

        private static void CompareSuccess(UserEntity expected, UserEntity actual)
        {
            Assert.NotNull(expected);
            Assert.NotNull(actual);
            Assert.Equal(expected.Email, actual.Email);
            Assert.Equal(expected.Name, actual.Name);
            Assert.Equal(expected.Passcode, actual.Passcode);
        }

        [Fact, TestPriority(2)]
        public async Task GetAllActivesTest()
        {
            var users = (await _sut.GetAllActivesAsync(1, 10))?.ToList();
            Assert.NotNull(users);
            var hasInactive = users.Exists(_ => !_.Active);
            Assert.False(hasInactive);
        }
        
        [Fact, TestPriority(3)]
        public async Task GetAllInactivesTest()
        {
            var users = (await _sut.GetAllInactivesAsync(1, 10))?.ToList();
            Assert.NotNull(users);
            var hasActive = users.Exists(_ => _.Active);
            Assert.False(hasActive);
        }

        [Theory, TestPriority(4)]
        [MemberData(nameof(GetUserForUpdate))]
        public async Task UpdateUserTest(UserEntity currentUser, UserEntity otherUser)
        {
            var selectedUser = await _sut.GetUserByEmailAsync(currentUser.Email);
            Assert.NotNull(selectedUser);
            otherUser.Id = selectedUser.Id;
            var updatedSuccessfully = await _sut.UpdateDataAsync(otherUser);
            Assert.True(updatedSuccessfully);
            updatedSuccessfully = await _sut.UpdateDataAsync(selectedUser);
            Assert.True(updatedSuccessfully);
        }
        
        [Fact, TestPriority(5)]
        public async Task ChangePasswordTest()
        {
            var users = (await _sut.GetAllActivesAsync(1, 1))?.ToList();
            Assert.NotNull(users);
            var user = users.First();
            var changed = await _sut.ChangePasswordByIdAsync(user.Id, "newPassword");
            Assert.True(changed);
        }
                
        [Theory, TestPriority(99)]
        [MemberData(nameof(GetValidUsers))]
        public async Task DeleteUserTest(UserEntity user)
        {
            var selectedUser = await _sut.GetUserByEmailAsync(user.Email);
            Assert.NotNull(selectedUser);
            var deleted = await _sut.DeleteDataByPkAsync(selectedUser.Id);
            Assert.True(deleted);
        }
        
        [Theory, TestPriority(-5)]
        [MemberData(nameof(GetValidUsers))]
        public async Task DeleteAllTest(UserEntity user)
        {
            var selectedUser = await _sut.GetUserByEmailAsync(user.Email);
            if (selectedUser == null)
                return;
            await _sut.DeleteDataByPkAsync(selectedUser.Id);
        }

        public static IEnumerable<object[]> GetValidUsers()
        {
            var unregisteredUsers = UserFixture.GetUnregisteredUsers();
            yield return new object[] { unregisteredUsers[0] };
            yield return new object[] { unregisteredUsers[1] };
            yield return new object[] { unregisteredUsers[2] };
            yield return new object[] { unregisteredUsers[3] };
            yield return new object[] { unregisteredUsers[4] };
            yield return new object[] { unregisteredUsers[5] };
            yield return new object[] { unregisteredUsers[6] };
            yield return new object[] { unregisteredUsers[7] };
            yield return new object[] { unregisteredUsers[8] };
            yield return new object[] { unregisteredUsers[9] };
        }

        public static IEnumerable<object[]> GetUserForUpdate()
        {
            yield return new object[] { UserFixture.GetUnregisteredUsers().First(), UserFixture.GetUserForUpdate() };
        }
    }
}
