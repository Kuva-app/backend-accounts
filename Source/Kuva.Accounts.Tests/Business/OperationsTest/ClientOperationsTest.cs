using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kuva.Accounts.Business.Exceptions;
using Kuva.Accounts.Business.Operations;
using Kuva.Accounts.Entities;
using Kuva.Accounts.Repository.Data.Interfaces;
using Kuva.Accounts.Tests.Fixtures;
using Moq;
using Xunit;

#nullable enable
namespace Kuva.Accounts.Tests.Business.OperationsTest
{
    public class ClientOperationsTest
    {
        private readonly Mock<IUserData> _userDataMock = new();

        [Fact]
        public async Task RegisteredSuccessfullyTest()
        {
            var expected = UserFixture.GetRegisteredUsers().First();

            _userDataMock.Reset();
            _userDataMock.Setup(s => s.AddDataAsync(It.IsAny<UserEntity>()))
                .ReturnsAsync(expected);

            var operation = new ClientOperations(_userDataMock.Object);

            var actual = await operation.Register(UserFixture.GetRegisteredUsers().First());

            Assert.NotNull(actual);
            Assert.Equal(UserLevels.Client, actual.UserLevelId);
            Assert.Equal(expected.Id, actual.Id);
        }

        [Theory]
        [MemberData(nameof(GetInvalidUser))]
        public async Task RegisteredThrowExceptionTest(UserEntity user, Type typeException)
        {
            _userDataMock.Reset();

            _userDataMock.Setup(s => s.AddDataAsync(It.IsAny<UserEntity>()))
                .ReturnsAsync(UserFixture.GetRegisteredUsers().First());

            var operation = new ClientOperations(_userDataMock.Object);

            var e = await Assert.ThrowsAsync(typeException, async () =>
            {
                await operation.Register(user);
            });
            
            Assert.IsType(typeException, e);
        }

        [Fact]
        public async Task UnregisterSuccessfullyTest()
        {
            _userDataMock.Reset();
            _userDataMock.Setup(s => s.DeleteDataByPkAsync(It.IsAny<long>()))
                .ReturnsAsync(true);

            var user = UserFixture.GetRegisteredUsers().First();

            var operation = new ClientOperations(_userDataMock.Object);

            var deleted = await operation.Unregister(user);

            Assert.True(deleted);
        }

        [Fact]
        public async Task UnregisterThrowException()
        {
            UserEntity? expected = null;
            
            _userDataMock.Reset();
            _userDataMock.Setup(s => s.DeleteDataByPkAsync(It.IsAny<UserEntity>()))
                .ReturnsAsync(true);
            
            var operation = new ClientOperations(_userDataMock.Object);
            var deleted = false;

            var e = await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
#pragma warning disable 8604
                deleted = await operation.Unregister(expected);
#pragma warning restore 8604
            });

            Assert.IsType<ArgumentNullException>(e);
            Assert.False(deleted);
        }

        public static IEnumerable<object[]> GetInvalidUser()
        {
            yield return new object[] { UserFixture.GetUserRegisterThrowException()[0], typeof(NullReferenceException) };
            yield return new object[] { UserFixture.GetUserRegisterThrowException()[1], typeof(RegisterException) };
        }

    }
}
