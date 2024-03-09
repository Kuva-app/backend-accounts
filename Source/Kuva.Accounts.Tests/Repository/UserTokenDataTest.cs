using System.Linq;
using System.Threading.Tasks;
using Kuva.Accounts.Entities;
using Kuva.Accounts.Repository.Data.Interfaces;
using Kuva.Accounts.Tests.Fixtures;
using Moq;

namespace Kuva.Accounts.Tests.Repository
{
    public class UserTokenDataTest : BaseRepositoryTest
    {
        private readonly Mock<IUserData> _userDataMock = new();
        private readonly Mock<IUserTokenData> _userTokenDataMock = new();

        [Fact]
        public async Task RequestTokenSuccessfullyTest()
        {
            _userDataMock.Reset();
            _userDataMock.Setup(s => s.GetUserByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(UserFixture.GetUnregisteredUsers().First());
            _userDataMock.Setup(s => s.DeleteDataByPkAsync(It.IsAny<string>()))
                .ReturnsAsync(true);

            _userTokenDataMock.Reset();
            _userTokenDataMock.Setup(s => s.AddDataAsync(It.IsAny<UserTokenEntity>()))
                .ReturnsAsync(UserTokenFixture.GetDefaultValuesFor(1));
            _userTokenDataMock.Setup(s => s.DeleteDataByPkAsync(It.IsAny<string>()))
                .ReturnsAsync(true);

            var user = UserFixture.GetUnregisteredUsers().First();
            var selectedUser = await _userDataMock.Object.GetUserByEmailAsync(user.Email);
            var newUser = selectedUser ?? await _userDataMock.Object.AddDataAsync(user);
            var expected = UserTokenFixture.GetDefaultValuesFor(newUser.Id);
            
            var actual = await _userTokenDataMock.Object.AddDataAsync(expected);
            
            Assert.NotNull(actual);
            Assert.True(!string.IsNullOrEmpty(actual.Id) || !string.IsNullOrWhiteSpace(actual.Id));
            Assert.True(!string.IsNullOrEmpty(actual.ConfirmationCode) || !string.IsNullOrWhiteSpace(actual.ConfirmationCode));
            
            var deleted = await _userTokenDataMock.Object.DeleteDataByPkAsync(actual.Id);
            Assert.True(deleted);
            
            deleted = await _userDataMock.Object.DeleteDataByPkAsync(newUser.Id);
            Assert.True(deleted);
        }
    }
}